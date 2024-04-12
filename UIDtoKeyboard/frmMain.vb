Imports PCSC
Imports PCSC.Iso7816
Imports PCSC.Monitoring

Public Class frmMain

    Private Shared ReadOnly _contextFactory As IContextFactory = ContextFactory.Instance
    Private _hContext As ISCardContext
    Dim readerName As String
    Dim readingMode As String
    Dim isstart As Boolean = False

    Dim exeFilePath As String = Application.StartupPath
    Dim iniFilePath As String = System.IO.Path.Combine(exeFilePath, "readingmode.ini")
    Dim logDirectory As String = System.IO.Path.Combine(exeFilePath, "Logs")
    Dim logFilePath As String = System.IO.Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}-log.txt")


    Function loadReaderList()
        Dim readerList As String()
        Try
            cbxReaderList.DataSource = Nothing

            _hContext = _contextFactory.Establish(SCardScope.System)
            readerList = _hContext.GetReaders()
            _hContext.Release()

            If readerList.Length > 0 Then
                cbxReaderList.DataSource = readerList

                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - The following reader is being used: " & readerList.ToString())
            Else
                MessageBox.Show("No card reader detected!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - No card reader detected!")
            End If

            Return True
        Catch ex As Exceptions.PCSCException
            MessageBox.Show("Error: getReaderList() : " & ex.Message & " (" & ex.SCardError.ToString() & ")")

            ' Write to log-file
            WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - Error: getReaderList() : " & ex.Message & " (" & ex.SCardError.ToString() & ")")
            Return False
        End Try
    End Function

    Dim monitor

    Private Sub startMonitor()
        If txtReadingMode.Text <> "" Then
            Dim monitorFactory As MonitorFactory = MonitorFactory.Instance
            monitor = monitorFactory.Create(SCardScope.System)
            AttachToAllEvents(monitor)
            monitor.Start(cbxReaderList.Text)

            readerName = cbxReaderList.Text
            readingMode = txtReadingMode.Text
            lblStatus.Text = "Running"
            lblStatus.ForeColor = Color.Green

            If chkLogging.Checked = True Then
                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - Monitoring has started")
            Else
            End If
        Else
            MessageBox.Show("Please first fill in the Reading Mode value in the textfield, otherwise it can not run.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub AttachToAllEvents(monitor As ISCardMonitor)
        AddHandler monitor.CardInserted, AddressOf cardInit
    End Sub

    Sub cardInit(eventName As SCardMonitor, unknown As CardStatusEventArgs)
        If readingMode = 1 OrElse readingMode = 2 Then
            SendUID4Byte()
        ElseIf readingMode = 3 OrElse readingMode = 4 Then
            sendUID7Byte()
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Checking if ini-file exist. If not, create an empty file.
        If Not System.IO.File.Exists(iniFilePath) Then
            ' Als het niet bestaat, maak het dan leeg aan
            System.IO.File.WriteAllText(iniFilePath, "")
        End If

        ' Fill in the location of the ini-file
        txtLocationIniFile.Text = iniFilePath

        ' Load Readingmode from ini-file, if exist fill it into the right textfield
        If System.IO.File.Exists(iniFilePath) Then
            Dim iniContents As String() = System.IO.File.ReadAllLines(iniFilePath)
            For Each line As String In iniContents
                If line.StartsWith("ReadingMode=") Then
                    Dim value As String = line.Split("="c)(1).Trim()
                    If IsNumeric(value) AndAlso CInt(value) >= 1 AndAlso CInt(value) <= 6 Then
                        txtReadingMode.Text = value
                        Exit For
                    End If
                End If
            Next
        End If

        ' Load Automatic from ini-file, if exist check the checkbox...but only when there is a value in Reading Mode.
        If txtReadingMode.Text <> "" Then
            If System.IO.File.Exists(iniFilePath) Then
                Dim iniContents As String() = System.IO.File.ReadAllLines(iniFilePath)
                For Each line As String In iniContents
                    If line.StartsWith("Automatic=") Then
                        Dim automateValue As String = line.Split("="c)(1).Trim().ToLower()
                        If automateValue = "yes" Then
                            chkAutomate.Checked = True
                        Else
                            chkAutomate.Checked = False
                        End If
                        Exit For
                    End If
                Next
            End If
        Else
            chkAutomate.Checked = False
            chkAutomate.Enabled = False
            btnStartMonitor.Enabled = False
        End If

        ' Checking if chkMinimizeOnStart is checked, if so...start the program minimized
        If System.IO.File.Exists(iniFilePath) Then
            Dim iniContents As String() = System.IO.File.ReadAllLines(iniFilePath)
            For Each line As String In iniContents
                If line.StartsWith("Minimized=") Then
                    Dim minimizedValue As String = line.Split("="c)(1).Trim().ToLower()
                    If minimizedValue = "yes" Then
                        chkMinimizedOnStart.Checked = True
                        Me.WindowState = FormWindowState.Minimized
                    Else
                        chkMinimizedOnStart.Checked = False
                    End If
                    Exit For
                End If
            Next
        End If

        ' Checking if chkLogging is checked, if so...enable logging
        If System.IO.File.Exists(iniFilePath) Then
            Dim iniContents As String() = System.IO.File.ReadAllLines(iniFilePath)
            For Each line As String In iniContents
                If line.StartsWith("LoggingEnabled=") Then
                    Dim loggingenabledValue As String = line.Split("="c)(1).Trim().ToLower()
                    If loggingenabledValue = "yes" Then
                        chkLogging.Checked = True
                    Else
                        chkLogging.Checked = False
                    End If
                    Exit For
                End If
            Next
        End If

        If chkLogging.Checked = True Then
            ' Create everything for logging
            CreateLoggingFolderandFile()

            ' Write to log-file
            WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - Program has been started")
        Else
        End If

        ' Load the readers
        loadReaderList()

        If chkAutomate.Checked Then
            btnStartMonitor_Click(Nothing, EventArgs.Empty)
        End If
    End Sub

    Private Sub btnRefreshReader_Click(sender As Object, e As EventArgs) Handles btnRefreshReader.Click
        loadReaderList()
    End Sub

    Private Sub btnStartMonitor_Click(sender As Object, e As EventArgs) Handles btnStartMonitor.Click
        If isstart = True Then
            monitor.Cancel()
        End If
        startMonitor()
        isstart = True

        btnStartMonitor.Enabled = False
    End Sub

    Private Sub btnStopMonitor_Click(sender As Object, e As EventArgs) Handles btnStopMonitor.Click
        If isstart = True Then
            monitor.Cancel()

            lblStatus.Text = "Stopped"
            lblStatus.ForeColor = Color.Red

            btnStartMonitor.Enabled = True

            If chkLogging.Checked = True Then
                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - Monitor has been stopped")
            Else
            End If
        End If
    End Sub
    Function SendUID4Byte()
        Try
            Using context = _contextFactory.Establish(SCardScope.System)
                Using rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any)
                    Using rfidReader.Transaction(SCardReaderDisposition.Leave)

                        Dim apdu As Byte() = {&HFF, &HCA, &H0, &H0, &H4}
                        Dim sendPci = SCardPCI.GetPci(rfidReader.Protocol)
                        Dim receivePci = New SCardPCI()

                        Dim receiveBuffer = New Byte(255) {}
                        Dim command = apdu.ToArray()
                        Dim bytesReceived = rfidReader.Transmit(sendPci, command, command.Length, receivePci, receiveBuffer, receiveBuffer.Length)
                        Dim responseApdu = New ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol)

                        If readingMode = 1 Then
                            Dim uid As String = BitConverter.ToString(responseApdu.GetData())
                            uid = uid.Replace("-", "")
                            'uid = uid.Substring(0, 14) ' Only the 14 Characters of the UID
                            SendKeys.SendWait(uid & "{ENTER}")

                            If chkLogging.Checked = True Then
                                ' Write to log-file
                                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'uid'-value has been send: " & uid.ToString())
                            Else
                            End If

                            System.Threading.Thread.Sleep(500) ' Wait 500 millisecondes (0,5 seconde)

                        ElseIf readingMode = 2 Then
                            Dim uid As Byte() = New Byte(3) {}
                            Dim revuid As Byte() = New Byte(3) {}
                            Array.Copy(responseApdu.GetData(), uid, 4)
                            Array.Copy(uid, revuid, 4)
                            Array.Reverse(revuid, 0, 4)

                            Dim uid2 As String = BitConverter.ToString(revuid)
                            uid2 = uid2.Replace("-", "")

                            SendKeys.SendWait(uid2 & "{ENTER}")

                            If chkLogging.Checked = True Then
                                ' Write to log-file
                                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'uid'-value has been send: " & uid2.ToString())
                            Else
                            End If

                            System.Threading.Thread.Sleep(500) ' Wait 500 millisecondes (0,5 seconde)
                        End If

                        ' Simulate an Enter-key
                        SendKeys.SendWait("{ENTER}")

                        If chkLogging.Checked = True Then
                            ' Write to log-file
                            WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'Enter'-key has been send")
                        Else
                        End If
                    End Using
                End Using
            End Using
        Catch
            'Error Handling should be developed
        End Try

        Return True
    End Function

    Function SendUID7Byte()
        Try
            Using context = _contextFactory.Establish(SCardScope.System)
                Using rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any)
                    Using rfidReader.Transaction(SCardReaderDisposition.Leave)

                        Dim apdu As Byte() = {&HFF, &HCA, &H0, &H0, &H7}
                        Dim sendPci = SCardPCI.GetPci(rfidReader.Protocol)
                        Dim receivePci = New SCardPCI()

                        Dim receiveBuffer = New Byte(255) {}
                        Dim command = apdu.ToArray()
                        Dim bytesReceived = rfidReader.Transmit(sendPci, command, command.Length, receivePci, receiveBuffer, receiveBuffer.Length)
                        Dim responseApdu = New ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol)

                        If readingMode = 3 Then
                            Dim uid As String = BitConverter.ToString(responseApdu.GetData())
                            uid = uid.Replace("-", "")
                            'uid = uid.Substring(0, 14) ' Only the first 14 characters of the UID
                            SendKeys.SendWait(uid & "{ENTER}")

                            If chkLogging.Checked = True Then
                                ' Write to log-file
                                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'uid'-value has been send: " & uid.ToString())
                            Else
                            End If

                            System.Threading.Thread.Sleep(500) ' Wait 500 millisecondes (0,5 seconde)

                        ElseIf readingMode = 4 Then
                            Dim uid As Byte() = New Byte(6) {}
                            Dim revuid As Byte() = New Byte(6) {}
                            Array.Copy(responseApdu.GetData(), uid, 7)
                            Array.Copy(uid, revuid, 7)
                            Array.Reverse(revuid, 0, 7)

                            Dim uid2 As String = BitConverter.ToString(revuid)
                            uid2 = uid2.Replace("-", "")

                            SendKeys.SendWait(uid2 & "{ENTER}")

                            If chkLogging.Checked = True Then
                                ' Write to log-file
                                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'uid'-value has been send: " & uid2.ToString())
                            Else
                            End If

                            System.Threading.Thread.Sleep(500) ' Wait 500 millisecondes (0,5 seconde)

                        End If

                        ' Simulate an Enter-key
                        SendKeys.SendWait("{ENTER}")

                        If chkLogging.Checked = True Then
                            ' Write to log-file
                            WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - 'Enter'-key has been send")
                        Else
                        End If
                    End Using
                End Using
            End Using
        Catch
            'Error Handling should be developed
        End Try

        Return True
    End Function
    Private Sub txtReadingMode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtReadingMode.KeyDown
        ' Only numbers 1 to 4, Backspace and Delete key are allowed
        If Not (e.KeyCode >= Keys.D1 AndAlso e.KeyCode <= Keys.D4) AndAlso Not (e.KeyCode >= Keys.NumPad1 AndAlso e.KeyCode <= Keys.NumPad4) AndAlso e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete Then
            e.SuppressKeyPress = True
        End If

        ' Checking if textlength i more than 1, and allow Backup and Delete to clear the text
        If txtReadingMode.Text.Length >= 1 AndAlso e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtReadingMode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtReadingMode.KeyUp
        ' Checking if there is a value in txtReadingMode. If not, disable automatic on start checkbox
        If txtReadingMode.Text = "" Then
            chkAutomate.Checked = False
            chkAutomate.Enabled = False
            btnStartMonitor.Enabled = False
        Else
            chkAutomate.Enabled = True
            btnStartMonitor.Enabled = True
        End If
    End Sub

    Private Sub btnFillIniFile_Click(sender As Object, e As EventArgs) Handles btnFillIniFile.Click
        Dim readingMode As Integer
        Dim iniFilePath As String = System.IO.Path.Combine(Application.StartupPath, "readingmode.ini")

        If Integer.TryParse(txtReadingMode.Text, readingMode) AndAlso readingMode >= 1 AndAlso readingMode <= 4 Then
            ' Checking if chkAutomate is checked
            Dim automateValue As String = If(chkAutomate.Checked, "yes", "no")
            Dim minimizedValue As String = If(chkMinimizedOnStart.Checked, "yes", "no")
            Dim loggingenabledValue As String = If(chkLogging.Checked, "yes", "no")

            ' Deleting existing values of ReadingMode and Automatic
            Dim iniContents As List(Of String) = New List(Of String)(System.IO.File.ReadAllLines(iniFilePath))
            For i As Integer = 0 To iniContents.Count - 1
                If iniContents(i).StartsWith("ReadingMode=") Then
                    iniContents.RemoveAt(i)
                    Exit For ' We found and removed the line, so exit the loop
                End If
            Next

            For i As Integer = 0 To iniContents.Count - 1
                If iniContents(i).StartsWith("Automatic=") Then
                    iniContents.RemoveAt(i)
                    Exit For ' We found and removed the line, so exit the loop
                End If
            Next

            For i As Integer = 0 To iniContents.Count - 1
                If iniContents(i).StartsWith("Minimized=") Then
                    iniContents.RemoveAt(i)
                    Exit For ' We found and removed the line, so exit the loop
                End If
            Next

            For i As Integer = 0 To iniContents.Count - 1
                If iniContents(i).StartsWith("LoggingEnabled=") Then
                    iniContents.RemoveAt(i)
                    Exit For ' We found and removed the line, so exit the loop
                End If
            Next

            ' Adding values ReadingMode, Automatic, Minimized and LoggingEnabled
            iniContents.Add("ReadingMode=" & readingMode.ToString())
            If chkAutomate.Checked Then
                If txtReadingMode.Text.Length <> 0 Then
                    iniContents.Add("Automatic=" & automateValue)
                Else
                    chkAutomate.Checked = False
                    chkAutomate.Enabled = False

                    MessageBox.Show("Please first fill in the Reading Mode value in the textfield, otherwise it can not be automated.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
            If chkMinimizedOnStart.Checked Then
                iniContents.Add("Minimized=" & minimizedValue)
            End If
            If chkLogging.Checked Then
                iniContents.Add("LoggingEnabled=" & loggingenabledValue)
            End If

            System.IO.File.WriteAllLines(iniFilePath, iniContents)
            MessageBox.Show("The value(s) has been added to the ini-file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If chkLogging.Checked = True Then
                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - The value(s) has been added to the ini-file.")
            Else
            End If
        Else
            MessageBox.Show("Invalid value for ReadingMode. Please fill in a value between 1 and 4.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnEmptyIniFile_Click(sender As Object, e As EventArgs) Handles btnEmptyIniFile.Click
        Dim iniFilePath As String = System.IO.Path.Combine(Application.StartupPath, "readingmode.ini")
        If System.IO.File.Exists(iniFilePath) Then
            System.IO.File.WriteAllText(iniFilePath, "")

            MessageBox.Show("The value(s) has been cleared from the ini-file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If chkLogging.Checked = True Then
                ' Write to log-file
                WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - The value(s) has been cleared from the ini-file.")
            Else
            End If
        Else
            MessageBox.Show("There is no ini-file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub WriteToLogFile(ByVal filePath As String, ByVal logText As String)
        Try
            ' Open het logbestand om te schrijven, als het niet bestaat wordt het gemaakt
            Using writer As System.IO.StreamWriter = New System.IO.StreamWriter(filePath, True)
                writer.WriteLine($"{DateTime.Now} - {logText}")
            End Using
        Catch ex As Exception
            Console.WriteLine($"Error with writing to log-file: {ex.Message}")
        End Try
    End Sub

    Private Sub CreateLoggingFolderandFile()
        ' Check if the Logs-directory exist, if not, create the directory
        If Not System.IO.Directory.Exists(logDirectory) Then
            System.IO.Directory.CreateDirectory(logDirectory)
        End If

        If chkLogging.Checked = True Then
            ' Write to log-file
            WriteToLogFile(logFilePath, $"{DateTime.Now:yyyyMMdd HH:mm:ss} - Logging has been enabled.")
        Else
        End If
    End Sub

    Private Sub chkLogging_CheckedChanged(sender As Object, e As EventArgs) Handles chkLogging.CheckedChanged
        If chkLogging.Checked = True Then
            CreateLoggingFolderandFile()
        Else
        End If
    End Sub
End Class
