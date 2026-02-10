Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

''' <summary>
''' Module helper untuk styling modern pada seluruh kontrol aplikasi.
''' </summary>
Public Module ModernUI

    ' ═══════════════════════ COLOR PALETTE ═══════════════════════
    Public ReadOnly ColorSidebar As Color = Color.FromArgb(27, 40, 56)
    Public ReadOnly ColorSidebarHover As Color = Color.FromArgb(44, 62, 80)
    Public ReadOnly ColorSidebarActive As Color = Color.FromArgb(52, 73, 94)
    Public ReadOnly ColorPrimary As Color = Color.FromArgb(52, 152, 219)
    Public ReadOnly ColorSuccess As Color = Color.FromArgb(39, 174, 96)
    Public ReadOnly ColorWarning As Color = Color.FromArgb(243, 156, 18)
    Public ReadOnly ColorDanger As Color = Color.FromArgb(231, 76, 60)
    Public ReadOnly ColorInfo As Color = Color.FromArgb(155, 89, 182)
    Public ReadOnly ColorBackground As Color = Color.FromArgb(236, 240, 245)
    Public ReadOnly ColorCard As Color = Color.White
    Public ReadOnly ColorTextPrimary As Color = Color.FromArgb(44, 62, 80)
    Public ReadOnly ColorTextSecondary As Color = Color.FromArgb(127, 140, 141)
    Public ReadOnly ColorTextLight As Color = Color.FromArgb(189, 195, 199)
    Public ReadOnly ColorBorder As Color = Color.FromArgb(218, 224, 230)
    Public ReadOnly ColorWhite As Color = Color.White
    Public ReadOnly ColorInputBg As Color = Color.FromArgb(248, 249, 250)
    Public ReadOnly ColorRowAlt As Color = Color.FromArgb(250, 251, 252)
    Public ReadOnly ColorSelectionBg As Color = Color.FromArgb(232, 245, 253)

    ' ═══════════════════════ FONTS ═══════════════════════
    Public ReadOnly FontHeader As New Font("Segoe UI", 20, FontStyle.Bold)
    Public ReadOnly FontSubHeader As New Font("Segoe UI Semibold", 14)
    Public ReadOnly FontTitle As New Font("Segoe UI Semibold", 12)
    Public ReadOnly FontBody As New Font("Segoe UI", 10)
    Public ReadOnly FontSmall As New Font("Segoe UI", 9)
    Public ReadOnly FontCardValue As New Font("Segoe UI", 28, FontStyle.Bold)
    Public ReadOnly FontCardLabel As New Font("Segoe UI", 10)
    Public ReadOnly FontSidebar As New Font("Segoe UI Semibold", 11)
    Public ReadOnly FontSidebarTitle As New Font("Segoe UI", 16, FontStyle.Bold)
    Public ReadOnly FontButton As New Font("Segoe UI Semibold", 10)
    Public ReadOnly FontInput As New Font("Segoe UI", 10)

    ' ═══════════════════════ BUTTON STYLING ═══════════════════════
    Public Sub StyleButton(btn As Button, bgColor As Color, Optional width As Integer = 130, Optional height As Integer = 38)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = bgColor
        btn.ForeColor = ColorWhite
        btn.Font = FontButton
        btn.Cursor = Cursors.Hand
        btn.Size = New Size(width, height)
        Dim original As Color = bgColor
        AddHandler btn.MouseEnter, Sub(s, e) btn.BackColor = ControlPaint.Light(original, 0.15F)
        AddHandler btn.MouseLeave, Sub(s, e) btn.BackColor = original
    End Sub

    ' ═══════════════════════ DATAGRIDVIEW STYLING ═══════════════════════
    Public Sub StyleDataGridView(dgv As DataGridView)
        dgv.BorderStyle = BorderStyle.None
        dgv.BackgroundColor = ColorCard
        dgv.GridColor = ColorBorder
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.RowHeadersVisible = False
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.Font = FontBody

        ' Header
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextSecondary
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10)
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 4, 8, 4)
        dgv.ColumnHeadersHeight = 44
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

        ' Rows
        dgv.DefaultCellStyle.Padding = New Padding(8, 4, 8, 4)
        dgv.DefaultCellStyle.ForeColor = ColorTextPrimary
        dgv.DefaultCellStyle.SelectionBackColor = ColorSelectionBg
        dgv.DefaultCellStyle.SelectionForeColor = ColorTextPrimary
        dgv.RowTemplate.Height = 40
        dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorRowAlt
    End Sub

    ' ═══════════════════════ INPUT STYLING ═══════════════════════
    Public Sub StyleTextBox(txt As TextBox)
        txt.Font = FontInput
        txt.BorderStyle = BorderStyle.FixedSingle
        txt.BackColor = ColorInputBg
        AddHandler txt.GotFocus, Sub(s, e) txt.BackColor = ColorWhite
        AddHandler txt.LostFocus, Sub(s, e) txt.BackColor = ColorInputBg
    End Sub

    Public Sub StyleComboBox(cmb As ComboBox)
        cmb.Font = FontInput
        cmb.FlatStyle = FlatStyle.Flat
        cmb.BackColor = ColorInputBg
        cmb.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Public Sub StyleDatePicker(dtp As DateTimePicker)
        dtp.Font = FontInput
        dtp.Format = DateTimePickerFormat.Short
    End Sub

    Public Sub StyleNumericUpDown(nud As NumericUpDown)
        nud.Font = FontInput
        nud.BorderStyle = BorderStyle.FixedSingle
    End Sub

    ' ═══════════════════════ CARD CREATION ═══════════════════════
    Public Function CreateCard(title As String, value As String, accentColor As Color, width As Integer, height As Integer) As Panel
        Dim card As New Panel()
        card.Size = New Size(width, height)
        card.BackColor = ColorCard

        Dim accent As New Panel()
        accent.Height = 4
        accent.Dock = DockStyle.Top
        accent.BackColor = accentColor

        Dim lblValue As New Label()
        lblValue.Text = value
        lblValue.Font = FontCardValue
        lblValue.ForeColor = ColorTextPrimary
        lblValue.AutoSize = True
        lblValue.Location = New Point(20, 20)
        lblValue.Tag = "cardValue"

        Dim lblTitle As New Label()
        lblTitle.Text = title
        lblTitle.Font = FontCardLabel
        lblTitle.ForeColor = ColorTextSecondary
        lblTitle.AutoSize = True
        lblTitle.Location = New Point(20, 75)

        card.Controls.Add(lblValue)
        card.Controls.Add(lblTitle)
        card.Controls.Add(accent)
        Return card
    End Function

    ' ═══════════════════════ LABEL HELPERS ═══════════════════════
    Public Function CreateSectionLabel(text As String) As Label
        Dim lbl As New Label()
        lbl.Text = text
        lbl.Font = FontTitle
        lbl.ForeColor = ColorTextPrimary
        lbl.AutoSize = True
        Return lbl
    End Function

    Public Function CreateFieldLabel(text As String) As Label
        Dim lbl As New Label()
        lbl.Text = text
        lbl.Font = FontBody
        lbl.ForeColor = ColorTextPrimary
        lbl.AutoSize = True
        Return lbl
    End Function

End Module
