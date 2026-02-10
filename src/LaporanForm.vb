Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Text

Public Class LaporanForm
    Inherits Form

    Private dgvLaporan As DataGridView
    Private dtpDari, dtpSampai As DateTimePicker
    Private cmbJenis As ComboBox
    Private btnFilter, btnExport, btnPrint As Button
    Private lblTotal As Label

    Public Sub New()
        Me.BackColor = ModernUI.ColorBackground
        Me.AutoScroll = True
        Me.Padding = New Padding(25)
        SetupFilterPanel()
        SetupGrid()
        LoadReport()
    End Sub

    Private Sub SetupFilterPanel()
        Dim pnl As New Panel()
        pnl.Location = New Point(25, 25) : pnl.Size = New Size(980, 100)
        pnl.BackColor = ModernUI.ColorCard
        pnl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim title As Label = ModernUI.CreateSectionLabel("Laporan & Riwayat")
        title.Location = New Point(20, 10) : pnl.Controls.Add(title)

        pnl.Controls.Add(FL("Jenis:", 20, 50))
        cmbJenis = New ComboBox()
        cmbJenis.Location = New Point(80, 47) : cmbJenis.Size = New Size(180, 30)
        ModernUI.StyleComboBox(cmbJenis)
        cmbJenis.Items.AddRange({"Stok Saat Ini", "Barang Masuk", "Barang Keluar", "Kartu Stok"})
        cmbJenis.SelectedIndex = 0 : pnl.Controls.Add(cmbJenis)

        pnl.Controls.Add(FL("Dari:", 280, 50))
        dtpDari = New DateTimePicker()
        dtpDari.Location = New Point(325, 47) : dtpDari.Size = New Size(140, 30)
        dtpDari.Value = DateTime.Now.AddMonths(-1)
        ModernUI.StyleDatePicker(dtpDari) : pnl.Controls.Add(dtpDari)

        pnl.Controls.Add(FL("Sampai:", 480, 50))
        dtpSampai = New DateTimePicker()
        dtpSampai.Location = New Point(540, 47) : dtpSampai.Size = New Size(140, 30)
        ModernUI.StyleDatePicker(dtpSampai) : pnl.Controls.Add(dtpSampai)

        btnFilter = New Button() : btnFilter.Text = "Tampilkan"
        btnFilter.Location = New Point(700, 45)
        ModernUI.StyleButton(btnFilter, ModernUI.ColorPrimary, 110) : AddHandler btnFilter.Click, Sub(s, e) LoadReport()
        pnl.Controls.Add(btnFilter)

        btnExport = New Button() : btnExport.Text = "Export CSV"
        btnExport.Location = New Point(820, 45)
        ModernUI.StyleButton(btnExport, ModernUI.ColorSuccess, 110) : AddHandler btnExport.Click, AddressOf ExportCSV
        pnl.Controls.Add(btnExport)

        Me.Controls.Add(pnl)
    End Sub

    Private Function FL(t As String, x As Integer, y As Integer) As Label
        Dim l As Label = ModernUI.CreateFieldLabel(t) : l.Location = New Point(x, y) : Return l
    End Function

    Private Sub SetupGrid()
        lblTotal = New Label()
        lblTotal.Font = New Font("Segoe UI Semibold", 11) : lblTotal.ForeColor = ModernUI.ColorTextPrimary
        lblTotal.Location = New Point(25, 140) : lblTotal.AutoSize = True : lblTotal.Text = ""
        Me.Controls.Add(lblTotal)

        dgvLaporan = New DataGridView()
        dgvLaporan.Location = New Point(25, 170) : dgvLaporan.Size = New Size(980, 480)
        dgvLaporan.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ModernUI.StyleDataGridView(dgvLaporan) : Me.Controls.Add(dgvLaporan)
    End Sub

    Private Sub LoadReport()
        Dim dari As String = dtpDari.Value.ToString("yyyy-MM-dd")
        Dim sampai As String = dtpSampai.Value.ToString("yyyy-MM-dd")
        Dim p As New Dictionary(Of String, Object) From {{"@dari", dari}, {"@sampai", sampai}}
        Dim dt As Data.DataTable = Nothing

        Select Case cmbJenis.SelectedIndex
            Case 0 ' Stok Saat Ini
                dt = DatabaseHelper.GetDataTable(
                    "SELECT b.KodeBarang AS 'Kode',b.NamaBarang AS 'Nama',COALESCE(k.NamaKategori,'') AS 'Kategori'," &
                    "b.Satuan,b.StokSaatIni AS 'Stok',b.StokMinimum AS 'Min',b.HargaBeli AS 'Harga Beli'," &
                    "(b.StokSaatIni*b.HargaBeli) AS 'Nilai Stok',b.Lokasi " &
                    "FROM Barang b LEFT JOIN Kategori k ON b.KategoriID=k.KategoriID ORDER BY b.NamaBarang")
                If dt.Rows.Count > 0 Then
                    Dim totalNilai As Double = 0
                    For Each row As Data.DataRow In dt.Rows
                        totalNilai += Convert.ToDouble(If(row("Nilai Stok") Is DBNull.Value, 0, row("Nilai Stok")))
                    Next
                    lblTotal.Text = $"Total {dt.Rows.Count} barang | Nilai Total: Rp {totalNilai:N0}"
                End If
            Case 1 ' Barang Masuk
                dt = DatabaseHelper.GetDataTable(
                    "SELECT bm.NoTransaksi AS 'No',b.KodeBarang AS 'Kode',b.NamaBarang AS 'Nama'," &
                    "bm.Jumlah,b.Satuan,bm.HargaBeli AS 'Harga',bm.Supplier,bm.TanggalMasuk AS 'Tanggal' " &
                    "FROM BarangMasuk bm JOIN Barang b ON bm.BarangID=b.BarangID " &
                    "WHERE date(bm.TanggalMasuk) BETWEEN @dari AND @sampai ORDER BY bm.TanggalMasuk DESC", p)
                lblTotal.Text = $"Total {dt.Rows.Count} transaksi masuk"
            Case 2 ' Barang Keluar
                dt = DatabaseHelper.GetDataTable(
                    "SELECT bk.NoTransaksi AS 'No',b.KodeBarang AS 'Kode',b.NamaBarang AS 'Nama'," &
                    "bk.Jumlah,b.Satuan,bk.Tujuan,bk.TanggalKeluar AS 'Tanggal' " &
                    "FROM BarangKeluar bk JOIN Barang b ON bk.BarangID=b.BarangID " &
                    "WHERE date(bk.TanggalKeluar) BETWEEN @dari AND @sampai ORDER BY bk.TanggalKeluar DESC", p)
                lblTotal.Text = $"Total {dt.Rows.Count} transaksi keluar"
            Case 3 ' Kartu Stok
                dt = DatabaseHelper.GetDataTable(
                    "SELECT b.KodeBarang AS 'Kode',b.NamaBarang AS 'Nama'," &
                    "COALESCE(SUM(CASE WHEN t.tipe='Masuk' THEN t.Jumlah ELSE 0 END),0) AS 'Total Masuk'," &
                    "COALESCE(SUM(CASE WHEN t.tipe='Keluar' THEN t.Jumlah ELSE 0 END),0) AS 'Total Keluar'," &
                    "b.StokSaatIni AS 'Stok Akhir',b.Satuan " &
                    "FROM Barang b LEFT JOIN (" &
                    "SELECT BarangID,Jumlah,'Masuk' AS tipe,TanggalMasuk AS tgl FROM BarangMasuk WHERE date(TanggalMasuk) BETWEEN @dari AND @sampai " &
                    "UNION ALL SELECT BarangID,Jumlah,'Keluar',TanggalKeluar FROM BarangKeluar WHERE date(TanggalKeluar) BETWEEN @dari AND @sampai" &
                    ") t ON b.BarangID=t.BarangID GROUP BY b.BarangID ORDER BY b.NamaBarang", p)
                lblTotal.Text = $"Kartu stok {dt.Rows.Count} barang"
        End Select

        dgvLaporan.DataSource = dt
    End Sub

    Private Sub ExportCSV(sender As Object, e As EventArgs)
        If dgvLaporan.Rows.Count = 0 Then MessageBox.Show("Tidak ada data!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) : Return

        Dim sfd As New SaveFileDialog()
        sfd.Filter = "CSV Files|*.csv" : sfd.FileName = $"Laporan_{cmbJenis.Text}_{DateTime.Now:yyyyMMdd}.csv"
        If sfd.ShowDialog() = DialogResult.OK Then
            Dim sb As New StringBuilder()
            ' Header
            For i As Integer = 0 To dgvLaporan.Columns.Count - 1
                If i > 0 Then sb.Append(",")
                sb.Append($"""{dgvLaporan.Columns(i).HeaderText}""")
            Next
            sb.AppendLine()
            ' Rows
            For Each row As DataGridViewRow In dgvLaporan.Rows
                For i As Integer = 0 To dgvLaporan.Columns.Count - 1
                    If i > 0 Then sb.Append(",")
                    sb.Append($"""{row.Cells(i).Value}""")
                Next
                sb.AppendLine()
            Next
            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8)
            MessageBox.Show($"File berhasil disimpan ke:{vbCrLf}{sfd.FileName}", "Export Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class
