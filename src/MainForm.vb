Imports System.Windows.Forms
Imports System.Drawing

Public Class MainForm
    Inherits Form

    Private pnlSidebar As Panel
    Private pnlContent As Panel
    Private pnlTopBar As Panel
    Private lblPageTitle As Label
    Private lblDateTime As Label
    Private tmrClock As Timer

    Private navItems As New List(Of Panel)
    Private activeNavPanel As Panel = Nothing
    Private currentForm As Form = Nothing

    Public Sub New()
        SetupForm()
        SetupSidebar()
        SetupMainArea()
        NavigateTo("Dashboard", 0)
    End Sub

    Private Sub SetupForm()
        Me.Text = "Sistem Manajemen Stok Barang"
        Me.Size = New Size(1300, 820)
        Me.MinimumSize = New Size(1100, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = ModernUI.ColorBackground
        Me.Font = ModernUI.FontBody
        Me.DoubleBuffered = True
    End Sub

    ' ════════════════════════════ SIDEBAR ════════════════════════════
    Private Sub SetupSidebar()
        pnlSidebar = New Panel()
        pnlSidebar.Width = 250
        pnlSidebar.Dock = DockStyle.Left
        pnlSidebar.BackColor = ModernUI.ColorSidebar

        ' Logo Area
        Dim pnlLogo As New Panel()
        pnlLogo.Height = 80
        pnlLogo.Dock = DockStyle.Top
        pnlLogo.BackColor = Color.FromArgb(22, 33, 47)
        Dim lblLogo As New Label()
        lblLogo.Text = "  STOK MANAGER"
        lblLogo.Font = ModernUI.FontSidebarTitle
        lblLogo.ForeColor = ModernUI.ColorWhite
        lblLogo.Dock = DockStyle.Fill
        lblLogo.TextAlign = ContentAlignment.MiddleLeft
        lblLogo.Padding = New Padding(15, 0, 0, 0)
        pnlLogo.Controls.Add(lblLogo)

        ' Separator
        Dim sep As New Panel()
        sep.Height = 1
        sep.Dock = DockStyle.Top
        sep.BackColor = Color.FromArgb(50, 65, 82)

        ' Navigation container
        Dim pnlNav As New Panel()
        pnlNav.Dock = DockStyle.Fill
        pnlNav.Padding = New Padding(0, 10, 0, 0)
        pnlNav.BackColor = Color.Transparent
        pnlNav.AutoScroll = True

        Dim menuItems() As String = {"Dashboard", "Master Barang", "Barang Masuk", "Barang Keluar", "Laporan", "Kategori"}
        Dim menuIcons() As String = {"◉", "▤", "▼", "▲", "▦", "⚙"}

        For i As Integer = 0 To menuItems.Length - 1
            Dim idx As Integer = i
            Dim nav As Panel = CreateNavItem(menuIcons(i), menuItems(i))
            nav.Location = New Point(0, 10 + i * 48)
            Dim handler As EventHandler = Sub(s, e) NavigateTo(menuItems(idx), idx)
            AddHandler nav.Click, handler
            For Each c As Control In nav.Controls
                AddHandler c.Click, handler
            Next
            navItems.Add(nav)
            pnlNav.Controls.Add(nav)
        Next

        ' Version label at bottom
        Dim lblVersion As New Label()
        lblVersion.Text = "v1.0.0"
        lblVersion.Font = ModernUI.FontSmall
        lblVersion.ForeColor = Color.FromArgb(80, 100, 120)
        lblVersion.Dock = DockStyle.Bottom
        lblVersion.Height = 40
        lblVersion.TextAlign = ContentAlignment.MiddleCenter

        pnlSidebar.Controls.Add(pnlNav)
        pnlSidebar.Controls.Add(lblVersion)
        pnlSidebar.Controls.Add(sep)
        pnlSidebar.Controls.Add(pnlLogo)

        Me.Controls.Add(pnlSidebar)
    End Sub

    Private Function CreateNavItem(icon As String, text As String) As Panel
        Dim pnl As New Panel()
        pnl.Size = New Size(250, 45)
        pnl.BackColor = Color.Transparent
        pnl.Cursor = Cursors.Hand

        Dim accent As New Panel()
        accent.Width = 4
        accent.Dock = DockStyle.Left
        accent.BackColor = Color.Transparent
        accent.Tag = "accent"

        Dim lbl As New Label()
        lbl.Text = $"     {icon}     {text}"
        lbl.Font = ModernUI.FontSidebar
        lbl.ForeColor = ModernUI.ColorTextLight
        lbl.Dock = DockStyle.Fill
        lbl.TextAlign = ContentAlignment.MiddleLeft
        lbl.Cursor = Cursors.Hand

        pnl.Controls.Add(lbl)
        pnl.Controls.Add(accent)

        AddHandler pnl.MouseEnter, Sub(s, e) HoverNav(pnl, True)
        AddHandler pnl.MouseLeave, Sub(s, e) HoverNav(pnl, False)
        AddHandler lbl.MouseEnter, Sub(s, e) HoverNav(pnl, True)
        AddHandler lbl.MouseLeave, Sub(s, e) HoverNav(pnl, False)

        Return pnl
    End Function

    Private Sub HoverNav(pnl As Panel, entering As Boolean)
        If pnl Is activeNavPanel Then Return
        pnl.BackColor = If(entering, ModernUI.ColorSidebarHover, Color.Transparent)
    End Sub

    Private Sub SetActiveNav(pnl As Panel)
        If activeNavPanel IsNot Nothing Then
            activeNavPanel.BackColor = Color.Transparent
            For Each c As Control In activeNavPanel.Controls
                If TypeOf c Is Panel AndAlso CStr(c.Tag) = "accent" Then c.BackColor = Color.Transparent
                If TypeOf c Is Label Then c.ForeColor = ModernUI.ColorTextLight
            Next
        End If

        activeNavPanel = pnl
        pnl.BackColor = ModernUI.ColorSidebarActive
        For Each c As Control In pnl.Controls
            If TypeOf c Is Panel AndAlso c.Tag IsNot Nothing AndAlso CStr(c.Tag) = "accent" Then c.BackColor = ModernUI.ColorPrimary
            If TypeOf c Is Label Then c.ForeColor = ModernUI.ColorWhite
        Next
    End Sub

    ' ════════════════════════════ TOP BAR + CONTENT ════════════════════════════
    Private Sub SetupMainArea()
        ' Top Bar
        pnlTopBar = New Panel()
        pnlTopBar.Height = 28
        pnlTopBar.Dock = DockStyle.Top
        pnlTopBar.BackColor = ModernUI.ColorCard
        pnlTopBar.Padding = New Padding(25, 0, 25, 0)

        Dim border As New Panel()
        border.Height = 1
        border.Dock = DockStyle.Bottom
        border.BackColor = ModernUI.ColorBorder

        lblPageTitle = New Label()
        lblPageTitle.Text = "Dashboard"
        lblPageTitle.Font = ModernUI.FontSubHeader
        lblPageTitle.ForeColor = ModernUI.ColorTextPrimary
        lblPageTitle.Dock = DockStyle.Left
        lblPageTitle.TextAlign = ContentAlignment.MiddleLeft
        lblPageTitle.AutoSize = True
        lblPageTitle.Padding = New Padding(5, 0, 0, 0)

        lblDateTime = New Label()
        lblDateTime.Font = ModernUI.FontBody
        lblDateTime.ForeColor = ModernUI.ColorTextSecondary
        lblDateTime.Dock = DockStyle.Right
        lblDateTime.TextAlign = ContentAlignment.MiddleRight
        lblDateTime.AutoSize = True

        tmrClock = New Timer()
        tmrClock.Interval = 1000
        AddHandler tmrClock.Tick, Sub(s, e)
                                      lblDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy   HH:mm:ss")
                                  End Sub
        tmrClock.Start()
        lblDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy   HH:mm:ss")

        pnlTopBar.Controls.Add(lblPageTitle)
        pnlTopBar.Controls.Add(lblDateTime)
        pnlTopBar.Controls.Add(border)

        ' Content Panel
        pnlContent = New Panel()
        pnlContent.Dock = DockStyle.Fill
        pnlContent.BackColor = ModernUI.ColorBackground
        pnlContent.Padding = New Padding(0)

        Me.Controls.Add(pnlContent)
        Me.Controls.Add(pnlTopBar)
        pnlContent.BringToFront()
    End Sub

    ' ════════════════════════════ NAVIGATION ════════════════════════════
    Private Sub NavigateTo(pageName As String, idx As Integer)
        SetActiveNav(navItems(idx))
        lblPageTitle.Text = pageName

        If currentForm IsNot Nothing Then
            currentForm.Close()
            currentForm.Dispose()
        End If

        Select Case pageName
            Case "Dashboard" : currentForm = New DashboardForm()
            Case "Master Barang" : currentForm = New MasterBarangForm()
            Case "Barang Masuk" : currentForm = New BarangMasukForm()
            Case "Barang Keluar" : currentForm = New BarangKeluarForm()
            Case "Laporan" : currentForm = New LaporanForm()
            Case "Kategori" : currentForm = New KategoriForm()
        End Select

        If currentForm IsNot Nothing Then
            currentForm.TopLevel = False
            currentForm.FormBorderStyle = FormBorderStyle.None
            currentForm.Dock = DockStyle.Fill
            pnlContent.Controls.Add(currentForm)
            currentForm.BringToFront()
            currentForm.Show()
        End If
    End Sub

End Class
