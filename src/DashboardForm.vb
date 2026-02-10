Imports System.Windows.Forms
Imports System.Drawing

Public Class DashboardForm
    Inherits Form

    Private cardTotalBarang As Panel
    Private cardStokRendah As Panel
    Private cardMasukHariIni As Panel
    Private cardKeluarHariIni As Panel
    Private dgvStokRendah As DataGridView
    Private dgvRecentTx As DataGridView
    Private tmrRefresh As Timer

    Public Sub New()
        SetupForm()
        SetupCards()
        SetupGrids()
        RefreshData()

        tmrRefresh = New Timer()
        tmrRefresh.Interval = 30000
        AddHandler tmrRefresh.Tick, Sub(s, e) RefreshData()
        tmrRefresh.Start()
    End Sub

    Private Sub SetupForm()
        Me.BackColor = ModernUI.ColorBackground
        Me.AutoScroll = True
        Me.Padding = New Padding(25)
    End Sub

    Private Sub SetupCards()
        Dim y As Integer = 25
        Dim cardW As Integer = 240
        Dim cardH As Integer = 100
        Dim gap As Integer = 20

        cardTotalBarang = ModernUI.CreateCard("Total Jenis Barang", "0", ModernUI.ColorPrimary, cardW, cardH)
        cardTotalBarang.Location = New Point(25, y)

        cardStokRendah = ModernUI.CreateCard("Stok Rendah", "0", ModernUI.ColorDanger, cardW, cardH)
        cardStokRendah.Location = New Point(25 + (cardW + gap), y)

        cardMasukHariIni = ModernUI.CreateCard("Masuk Hari Ini", "0", ModernUI.ColorSuccess, cardW, cardH)
        cardMasukHariIni.Location = New Point(25 + 2 * (cardW + gap), y)

        cardKeluarHariIni = ModernUI.CreateCard("Keluar Hari Ini", "0", ModernUI.ColorWarning, cardW, cardH)
        cardKeluarHariIni.Location = New Point(25 + 3 * (cardW + gap), y)

        Me.Controls.AddRange({cardTotalBarang, cardStokRendah, cardMasukHariIni, cardKeluarHariIni})
    End Sub

    Private Sub SetupGrids()
        Dim y As Integer = 150

        ' Stok Rendah Section
        Dim lblStokRendah As Label = ModernUI.CreateSectionLabel("⚠ Barang Dengan Stok Rendah")
        lblStokRendah.Location = New Point(25, y)
        Me.Controls.Add(lblStokRendah)

        dgvStokRendah = New DataGridView()
        dgvStokRendah.Location = New Point(25, y + 30)
        dgvStokRendah.Size = New Size(980, 200)
        dgvStokRendah.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ModernUI.StyleDataGridView(dgvStokRendah)
        Me.Controls.Add(dgvStokRendah)

        ' Recent Transactions Section
        Dim y2 As Integer = y + 250
        Dim lblRecent As Label = ModernUI.CreateSectionLabel("📋 Transaksi Terbaru")
        lblRecent.Location = New Point(25, y2)
        Me.Controls.Add(lblRecent)

        dgvRecentTx = New DataGridView()
        dgvRecentTx.Location = New Point(25, y2 + 30)
        dgvRecentTx.Size = New Size(980, 220)
        dgvRecentTx.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ModernUI.StyleDataGridView(dgvRecentTx)
        Me.Controls.Add(dgvRecentTx)
    End Sub

    Public Sub RefreshData()
        Try
            ' Card values
            Dim totalBarang As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Barang"))
            Dim stokRendah As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM Barang WHERE StokSaatIni <= StokMinimum AND StokMinimum > 0"))
            Dim today As String = DateTime.Now.ToString("yyyy-MM-dd")
            Dim masukHariIni As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM BarangMasuk WHERE date(TanggalMasuk) = @today", New Dictionary(Of String, Object) From {{"@today", today}}))
            Dim keluarHariIni As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM BarangKeluar WHERE date(TanggalKeluar) = @today", New Dictionary(Of String, Object) From {{"@today", today}}))

            UpdateCardValue(cardTotalBarang, totalBarang.ToString())
            UpdateCardValue(cardStokRendah, stokRendah.ToString())
            UpdateCardValue(cardMasukHariIni, masukHariIni.ToString())
            UpdateCardValue(cardKeluarHariIni, keluarHariIni.ToString())

            ' Stok Rendah Grid
            Dim dtRendah = DatabaseHelper.GetDataTable(
                "SELECT b.KodeBarang AS 'Kode', b.NamaBarang AS 'Nama Barang', " &
                "COALESCE(k.NamaKategori,'') AS 'Kategori', b.StokSaatIni AS 'Stok', " &
                "b.StokMinimum AS 'Minimum', b.Satuan " &
                "FROM Barang b LEFT JOIN Kategori k ON b.KategoriID = k.KategoriID " &
                "WHERE b.StokSaatIni <= b.StokMinimum AND b.StokMinimum > 0 " &
                "ORDER BY b.StokSaatIni ASC LIMIT 20")
            dgvStokRendah.DataSource = dtRendah

            ' Recent Transactions
            Dim dtRecent = DatabaseHelper.GetDataTable(
                "SELECT 'Masuk' AS Tipe, bm.NoTransaksi, b.NamaBarang AS 'Nama Barang', " &
                "bm.Jumlah, bm.TanggalMasuk AS Tanggal, bm.Supplier AS 'Supplier/Tujuan' " &
                "FROM BarangMasuk bm JOIN Barang b ON bm.BarangID = b.BarangID " &
                "UNION ALL " &
                "SELECT 'Keluar' AS Tipe, bk.NoTransaksi, b.NamaBarang, " &
                "bk.Jumlah, bk.TanggalKeluar AS Tanggal, bk.Tujuan " &
                "FROM BarangKeluar bk JOIN Barang b ON bk.BarangID = b.BarangID " &
                "ORDER BY Tanggal DESC LIMIT 10")
            dgvRecentTx.DataSource = dtRecent
        Catch ex As Exception
            ' Silently handle on first load when tables might be empty
        End Try
    End Sub

    Private Sub UpdateCardValue(card As Panel, value As String)
        For Each c As Control In card.Controls
            If TypeOf c Is Label AndAlso c.Tag IsNot Nothing AndAlso CStr(c.Tag) = "cardValue" Then
                c.Text = value
                Exit For
            End If
        Next
    End Sub

End Class
