<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cbxReaderList = New System.Windows.Forms.ComboBox()
        Me.btnRefreshReader = New System.Windows.Forms.Button()
        Me.btnStartMonitor = New System.Windows.Forms.Button()
        Me.txtInputSpace = New System.Windows.Forms.TextBox()
        Me.btnStopMonitor = New System.Windows.Forms.Button()
        Me.lblInst1 = New System.Windows.Forms.Label()
        Me.lblInst2 = New System.Windows.Forms.Label()
        Me.txtReadingMode = New System.Windows.Forms.TextBox()
        Me.lblReadingMode = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLocationIniFile = New System.Windows.Forms.TextBox()
        Me.chkAutomate = New System.Windows.Forms.CheckBox()
        Me.btnFillIniFile = New System.Windows.Forms.Button()
        Me.btnEmptyIniFile = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.chkMinimizedOnStart = New System.Windows.Forms.CheckBox()
        Me.chkLogging = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cbxReaderList
        '
        Me.cbxReaderList.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.cbxReaderList.FormattingEnabled = True
        Me.cbxReaderList.Location = New System.Drawing.Point(25, 22)
        Me.cbxReaderList.Name = "cbxReaderList"
        Me.cbxReaderList.Size = New System.Drawing.Size(400, 26)
        Me.cbxReaderList.TabIndex = 0
        '
        'btnRefreshReader
        '
        Me.btnRefreshReader.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnRefreshReader.Location = New System.Drawing.Point(431, 22)
        Me.btnRefreshReader.Name = "btnRefreshReader"
        Me.btnRefreshReader.Size = New System.Drawing.Size(87, 27)
        Me.btnRefreshReader.TabIndex = 2
        Me.btnRefreshReader.Text = "Refresh"
        Me.btnRefreshReader.UseVisualStyleBackColor = True
        '
        'btnStartMonitor
        '
        Me.btnStartMonitor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnStartMonitor.Location = New System.Drawing.Point(22, 91)
        Me.btnStartMonitor.Name = "btnStartMonitor"
        Me.btnStartMonitor.Size = New System.Drawing.Size(496, 32)
        Me.btnStartMonitor.TabIndex = 3
        Me.btnStartMonitor.Text = "Start Monitor"
        Me.btnStartMonitor.UseVisualStyleBackColor = True
        '
        'txtInputSpace
        '
        Me.txtInputSpace.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtInputSpace.Location = New System.Drawing.Point(22, 155)
        Me.txtInputSpace.Multiline = True
        Me.txtInputSpace.Name = "txtInputSpace"
        Me.txtInputSpace.Size = New System.Drawing.Size(496, 384)
        Me.txtInputSpace.TabIndex = 4
        '
        'btnStopMonitor
        '
        Me.btnStopMonitor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnStopMonitor.Location = New System.Drawing.Point(431, 55)
        Me.btnStopMonitor.Name = "btnStopMonitor"
        Me.btnStopMonitor.Size = New System.Drawing.Size(87, 27)
        Me.btnStopMonitor.TabIndex = 5
        Me.btnStopMonitor.Text = "Stop"
        Me.btnStopMonitor.UseVisualStyleBackColor = True
        '
        'lblInst1
        '
        Me.lblInst1.AutoSize = True
        Me.lblInst1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblInst1.Location = New System.Drawing.Point(551, 30)
        Me.lblInst1.Name = "lblInst1"
        Me.lblInst1.Size = New System.Drawing.Size(104, 18)
        Me.lblInst1.TabIndex = 6
        Me.lblInst1.Text = "Reading Mode"
        '
        'lblInst2
        '
        Me.lblInst2.AutoSize = True
        Me.lblInst2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblInst2.Location = New System.Drawing.Point(567, 59)
        Me.lblInst2.Name = "lblInst2"
        Me.lblInst2.Size = New System.Drawing.Size(217, 72)
        Me.lblInst2.TabIndex = 7
        Me.lblInst2.Text = "1- Card UID (4 Byte)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2- Card UID (4 Byte + Reverse)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3- Card UID (7 Byte)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4- Ca" &
    "rd UID (7 Byte + Reverse) "
        '
        'txtReadingMode
        '
        Me.txtReadingMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtReadingMode.Location = New System.Drawing.Point(136, 56)
        Me.txtReadingMode.Name = "txtReadingMode"
        Me.txtReadingMode.Size = New System.Drawing.Size(289, 24)
        Me.txtReadingMode.TabIndex = 8
        '
        'lblReadingMode
        '
        Me.lblReadingMode.AutoSize = True
        Me.lblReadingMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblReadingMode.Location = New System.Drawing.Point(22, 59)
        Me.lblReadingMode.Name = "lblReadingMode"
        Me.lblReadingMode.Size = New System.Drawing.Size(108, 18)
        Me.lblReadingMode.TabIndex = 9
        Me.lblReadingMode.Text = "Reading Mode:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 19.0!)
        Me.Label2.Location = New System.Drawing.Point(549, 357)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(237, 30)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Automation Options"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label1.Location = New System.Drawing.Point(438, 542)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 18)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Location ini-file:"
        '
        'txtLocationIniFile
        '
        Me.txtLocationIniFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtLocationIniFile.Location = New System.Drawing.Point(554, 539)
        Me.txtLocationIniFile.Name = "txtLocationIniFile"
        Me.txtLocationIniFile.Size = New System.Drawing.Size(244, 24)
        Me.txtLocationIniFile.TabIndex = 19
        '
        'chkAutomate
        '
        Me.chkAutomate.AutoSize = True
        Me.chkAutomate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
        Me.chkAutomate.Location = New System.Drawing.Point(554, 491)
        Me.chkAutomate.Name = "chkAutomate"
        Me.chkAutomate.Size = New System.Drawing.Size(144, 22)
        Me.chkAutomate.TabIndex = 18
        Me.chkAutomate.Text = "Automate on start"
        Me.chkAutomate.UseVisualStyleBackColor = True
        '
        'btnFillIniFile
        '
        Me.btnFillIniFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnFillIniFile.Location = New System.Drawing.Point(554, 435)
        Me.btnFillIniFile.Name = "btnFillIniFile"
        Me.btnFillIniFile.Size = New System.Drawing.Size(231, 27)
        Me.btnFillIniFile.TabIndex = 17
        Me.btnFillIniFile.Text = "Write all values to ini-file"
        Me.btnFillIniFile.UseVisualStyleBackColor = True
        '
        'btnEmptyIniFile
        '
        Me.btnEmptyIniFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnEmptyIniFile.Location = New System.Drawing.Point(554, 402)
        Me.btnEmptyIniFile.Name = "btnEmptyIniFile"
        Me.btnEmptyIniFile.Size = New System.Drawing.Size(231, 27)
        Me.btnEmptyIniFile.TabIndex = 16
        Me.btnEmptyIniFile.Text = "Empty ini-file"
        Me.btnEmptyIniFile.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 18)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Status:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(82, 129)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(63, 18)
        Me.lblStatus.TabIndex = 23
        Me.lblStatus.Text = "Stopped"
        '
        'chkMinimizedOnStart
        '
        Me.chkMinimizedOnStart.AutoSize = True
        Me.chkMinimizedOnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
        Me.chkMinimizedOnStart.Location = New System.Drawing.Point(554, 468)
        Me.chkMinimizedOnStart.Name = "chkMinimizedOnStart"
        Me.chkMinimizedOnStart.Size = New System.Drawing.Size(140, 22)
        Me.chkMinimizedOnStart.TabIndex = 24
        Me.chkMinimizedOnStart.Text = "Minimize on start"
        Me.chkMinimizedOnStart.UseVisualStyleBackColor = True
        '
        'chkLogging
        '
        Me.chkLogging.AutoSize = True
        Me.chkLogging.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!)
        Me.chkLogging.Location = New System.Drawing.Point(554, 512)
        Me.chkLogging.Name = "chkLogging"
        Me.chkLogging.Size = New System.Drawing.Size(128, 22)
        Me.chkLogging.TabIndex = 25
        Me.chkLogging.Text = "Enable Logging"
        Me.chkLogging.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(810, 567)
        Me.Controls.Add(Me.chkLogging)
        Me.Controls.Add(Me.chkMinimizedOnStart)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtLocationIniFile)
        Me.Controls.Add(Me.chkAutomate)
        Me.Controls.Add(Me.btnFillIniFile)
        Me.Controls.Add(Me.btnEmptyIniFile)
        Me.Controls.Add(Me.lblReadingMode)
        Me.Controls.Add(Me.txtReadingMode)
        Me.Controls.Add(Me.lblInst2)
        Me.Controls.Add(Me.lblInst1)
        Me.Controls.Add(Me.btnStopMonitor)
        Me.Controls.Add(Me.txtInputSpace)
        Me.Controls.Add(Me.btnStartMonitor)
        Me.Controls.Add(Me.btnRefreshReader)
        Me.Controls.Add(Me.cbxReaderList)
        Me.Name = "frmMain"
        Me.Text = "UIDtoKeyboard"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbxReaderList As ComboBox
    Friend WithEvents btnRefreshReader As Button
    Friend WithEvents btnStartMonitor As Button
    Friend WithEvents txtInputSpace As TextBox
    Friend WithEvents btnStopMonitor As Button
    Friend WithEvents lblInst1 As Label
    Friend WithEvents lblInst2 As Label
    Friend WithEvents txtReadingMode As TextBox
    Friend WithEvents lblReadingMode As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLocationIniFile As TextBox
    Friend WithEvents chkAutomate As CheckBox
    Friend WithEvents btnFillIniFile As Button
    Friend WithEvents btnEmptyIniFile As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents chkMinimizedOnStart As CheckBox
    Friend WithEvents chkLogging As CheckBox
End Class
