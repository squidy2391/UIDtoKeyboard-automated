Imports PCSC
Imports PCSC.Iso7816
Imports PCSC.Monitoring

Public Class frmMain

    Private Shared ReadOnly _contextFactory As IContextFactory = ContextFactory.Instance
    Private _hContext As ISCardContext
    Dim readerName As String
    Dim readingMode As String
    Dim isstart As Boolean = False

    Function loadReaderList()
        Dim readerList As String()
        Try
            cbxReaderList.DataSource = Nothing

            _hContext = _contextFactory.Establish(SCardScope.System)
            readerList = _hContext.GetReaders()
            _hContext.Release()

            If readerList.Length > 0 Then
                cbxReaderList.DataSource = readerList
            Else
                MessageBox.Show("No card reader detected!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return True
        Catch ex As Exceptions.PCSCException
            MessageBox.Show("Error: getReaderList() : " & ex.Message & " (" & ex.SCardError.ToString() & ")")
            Return False
        End Try
    End Function

    Dim monitor

    Private Sub startMonitor()
        Dim monitorFactory As MonitorFactory = MonitorFactory.Instance
        monitor = monitorFactory.Create(SCardScope.System)
        AttachToAllEvents(monitor)
        monitor.Start(cbxReaderList.Text)

        readerName = cbxReaderList.Text
        readingMode = txtReadingMode.Text
    End Sub

    Private Sub AttachToAllEvents(monitor As ISCardMonitor)
        AddHandler monitor.CardInserted, AddressOf cardInit
    End Sub

    Sub cardInit(eventName As SCardMonitor, unknown As CardStatusEventArgs)
        If readingMode = 1 OrElse readingMode = 2 Then
            SendUID4Byte()
        ElseIf readingMode = 3 OrElse readingMode = 4 Then
            SendUID7Byte()
        ElseIf readingMode = 5 OrElse readingMode = 6 Then
            SendUID8H10D()
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim iniFilePath As String = System.IO.Path.Combine(Application.StartupPath, "readingmode.ini")

        ' Load the readers
        loadReaderList()

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
        End If

        ' Checking if chkAutomate is checked, if so...start the monitor
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
    End Sub

    Private Sub btnStopMonitor_Click(sender As Object, e As EventArgs) Handles btnStopMonitor.Click
        If isstart = True Then
            monitor.Cancel()
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

                            SendKeys.SendWait(uid + "{ENTER}")
                        ElseIf readingMode = 2 Then
                            Dim uid As Byte() = New Byte(3) {}
                            Dim revuid As Byte() = New Byte(3) {}
                            Array.Copy(responseApdu.GetData(), uid, 4)
                            Array.Copy(uid, revuid, 4)
                            Array.Reverse(revuid, 0, 4)

                            Dim uid2 As String = BitConverter.ToString(revuid)
                            uid2 = uid2.Replace("-", "")

                            SendKeys.SendWait(uid2 + "{ENTER}")
                        End If
                    End Using
                End Using
            End Using
        Catch
            'Error Handling should be developed
        End Try

        Return True
    End Function

    Function SendUID8H10D()
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


                        Dim uid As String
                        If readingMode = 6 Then

                            uid = BitConverter.ToUInt32(responseApdu.GetData(), 0)

                        ElseIf readingMode = 5 Then
                            Dim revuid As Byte() = New Byte(4) {}

                            Array.Copy(responseApdu.GetData(), revuid, 4)
                            Array.Reverse(revuid, 0, 4)

                            uid = BitConverter.ToUInt32(revuid, 0)

                        End If
                        SendKeys.SendWait(uid + "{ENTER}")
                    End Using
                End Using
            End Using
        Catch
            Console.WriteLine("Erreur 8H10D")
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

                            SendKeys.SendWait(uid + "{ENTER}")
                        ElseIf readingMode = 4 Then
                            Dim uid As Byte() = New Byte(6) {}
                            Dim revuid As Byte() = New Byte(6) {}
                            Array.Copy(responseApdu.GetData(), uid, 7)
                            Array.Copy(uid, revuid, 7)
                            Array.Reverse(revuid, 0, 7)

                            Dim uid2 As String = BitConverter.ToString(revuid)
                            uid2 = uid2.Replace("-", "")

                            SendKeys.SendWait(uid2 + "{ENTER}")
                        End If
                    End Using
                End Using
            End Using
        Catch
            'Error Handling should be developed
        End Try

        Return True
    End Function

    Private Sub btnEmptyIniFile_Click(sender As Object, e As EventArgs) Handles btnEmptyIniFile.Click
        Dim iniFilePath As String = System.IO.Path.Combine(Application.StartupPath, "readingmode.ini")
        If System.IO.File.Exists(iniFilePath) Then
            System.IO.File.WriteAllText(iniFilePath, "")
            txtReadingMode.Clear()

            MessageBox.Show("The value(s) has been cleared from the ini-file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("There is no ini-file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnFillIniFile_Click(sender As Object, e As EventArgs) Handles btnFillIniFile.Click
        Dim readingMode As Integer
        Dim iniFilePath As String = System.IO.Path.Combine(Application.StartupPath, "readingmode.ini")

        If Integer.TryParse(txtReadingMode.Text, readingMode) AndAlso readingMode >= 1 AndAlso readingMode <= 6 Then
            ' Checking if chkAutomate is checked
            Dim automateValue As String = If(chkAutomate.Checked, "yes", "no")

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

            ' Adding values ReadingMode and Automatic
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

            System.IO.File.WriteAllLines(iniFilePath, iniContents)
            MessageBox.Show("The value(s) has been added to the ini-file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Invalid value for ReadingMode. Please fill in a value between 1 and 6.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub txtReadingMode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtReadingMode.KeyDown
        ' Only numbers 1 to 6, Backspace and Delete key are allowed
        If Not (e.KeyCode >= Keys.D1 AndAlso e.KeyCode <= Keys.D6) AndAlso Not (e.KeyCode >= Keys.NumPad1 AndAlso e.KeyCode <= Keys.NumPad6) AndAlso e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete Then
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
        Else
            chkAutomate.Enabled = True
        End If
    End Sub
End Class
