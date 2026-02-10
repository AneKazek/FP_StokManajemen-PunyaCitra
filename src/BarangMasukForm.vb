Imports System.Windows.Forms
Imports System.Drawing

Public Class BarangMasukForm
    Inherits Form

    Private dgvMasuk As DataGridView
    Private txtNoTransaksi, txtSupplier, txtKeterangan As TextBox
    Private cmbBarang As ComboBox
    Private nudJumlah As NumericUpDown
    Private nudHargaBeli As NumericUpDown
    Private dtpTanggal As DateTimePicker
    Private btnSimpan, btnHapus, btnBatal As Button

    Public Sub New()
        Me.BackColor = ModernUI.ColorBackground
        Me.AutoScroll = True
        Me.Padding = New Padding(25)
        SetupInputPanel()
        SetupGrid()
        LoadBarang()
        LoadData()
        GenerateNo()
    End Sub

    Private Sub SetupInputPanel()
        Dim pnl As New Panel()
        pnl.Location = New Point(25, 25)
        pnl.Size = New Size(980, 250)
        pnl.BackColor = ModernUI.ColorCard
        pnl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim title As Label = ModernUI.CreateSectionLabel("▼ Form Barang Masuk")
        title.Location = New Point(20, 15)
        pnl.Controls.Add(title)

        ' No Transaksi
        pnl.Controls.Add(MakeLabel("No. Transaksi:", 20, 53))
        txtNoTransaksi = New TextBox()
        txtNoTransaksi.Location = New Point(150, 50)
        txtNoTransaksi.Size = New Size(200, 30)
        txtNoTransaksi.ReadOnly = True
        ModernUI.StyleTextBox(txtNoTransaksi)
        pnl.Controls.Add(txtNoTransaksi)

        ' Tanggal
        pnl.Controls.Add(MakeLabel("Tanggal:", 380, 53))
        dtpTanggal = New DateTimePicker()
        dtpTanggal.Location = New Point(460, 50)
        dtpTanggal.Size = New Size(180, 30)
        ModernUI.StyleDatePicker(dtpTanggal)
        pnl.Controls.Add(dtpTanggal)

        ' Barang
        pnl.Controls.Add(MakeLabel("Barang:", 20, 93))
        cmbBarang = New ComboBox()
        cmbBarang.Location = New Point(150, 90)
        cmbBarang.Size = New Size(350, 30)
        ModernUI.StyleComboBox(cmbBarang)
        pnl.Controls.Add(cmbBarang)

        ' Jumlah
        pnl.Controls.Add(MakeLabel("Jumlah:", 520, 93))
        nudJumlah = New NumericUpDown()
        nudJumlah.Location = New Point(590, 90)
        nudJumlah.Size = New Size(100, 30)
        nudJumlah.Minimum = 1
        nudJumlah.Maximum = 999999
        ModernUI.StyleNumericUpDown(nudJumlah)
        pnl.Controls.Add(nudJumlah)

        ' Harga Beli
        pnl.Controls.Add(MakeLabel("Harga Beli:", 710, 93))
        nudHargaBeli = New NumericUpDown()
        nudHargaBeli.Location = New Point(800, 90)
        nudHargaBeli.Size = New Size(150, 30)
        nudHargaBeli.Maximum = 999999999
        nudHargaBeli.ThousandsSeparator = True
        ModernUI.StyleNumericUpDown(nudHargaBeli)
        pnl.Controls.Add(nudHargaBeli)

        ' Supplier
        pnl.Controls.Add(MakeLabel("Supplier:", 20, 133))
        txtSupplier = New TextBox()
        txtSupplier.Location = New Point(150, 130)
        txtSupplier.Size = New Size(250, 30)
        ModernUI.StyleTextBox(txtSupplier)
        pnl.Controls.Add(txtSupplier)

        ' Keterangan
        pnl.Controls.Add(MakeLabel("Keterangan:", 420, 133))
        txtKeterangan = New TextBox()
        txtKeterangan.Location = New Point(530, 130)
        txtKeterangan.Size = New Size(420, 30)
        ModernUI.StyleTextBox(txtKeterangan)
        pnl.Controls.Add(txtKeterangan)

        ' Buttons
        btnSimpan = New Button() : btnSimpan.Text = "💾 Simpan Transaksi"
        btnSimpan.Location = New Point(20, 175)
        ModernUI.StyleButton(btnSimpan, ModernUI.ColorSuccess, 180)
        AddHandler btnSimpan.Click, AddressOf BtnSimpan_Click
        pnl.Controls.Add(btnSimpan)

        btnBatal = New Button() : btnBatal.Text = "✖ Batal"
        btnBatal.Location = New Point(210, 175)
        ModernUI.StyleButton(btnBatal, Color.FromArgb(149, 165, 166))
        AddHandler btnBatal.Click, AddressOf BtnBatal_Click
        pnl.Controls.Add(btnBatal)

        Me.Controls.Add(pnl)
    End Sub

    Private Function MakeLabel(text As String, x As Integer, y As Integer) As Label
        Dim lbl As Label = ModernUI.CreateFieldLabel(text)
        lbl.Location = New Point(x, y)
        Return lbl
    End Function

    Private Sub SetupGrid()
        Dim lblHist As Label = ModernUI.CreateSectionLabel("📋 Riwayat Barang Masuk")
        lblHist.Location = New Point(25, 290)
        Me.Controls.Add(lblHist)

        btnHapus = New Button() : btnHapus.Text = "🗑 Hapus Transaksi"
        btnHapus.Location = New Point(830, 285)
        ModernUI.StyleButton(btnHapus, ModernUI.ColorDanger, 170)
        AddHandler btnHapus.Click, AddressOf BtnHapus_Click
        Me.Controls.Add(btnHapus)

        dgvMasuk = New DataGridView()
        dgvMasuk.Location = New Point(25, 320)
        dgvMasuk.Size = New Size(980, 350)
        dgvMasuk.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ModernUI.StyleDataGridView(dgvMasuk)
        Me.Controls.Add(dgvMasuk)
    End Sub

    Private Sub LoadBarang()
        cmbBarang.Items.Clear()
        cmbBarang.Items.Add("-- Pilih Barang --")
        Dim dt = DatabaseHelper.GetDataTable("SELECT BarangID, KodeBarang, NamaBarang, Satuan FROM Barang ORDER BY NamaBarang")
        For Each row As Data.DataRow In dt.Rows
            cmbBarang.Items.Add($"{row("BarangID")}|{row("KodeBarang")} - {row("NamaBarang")} ({row("Satuan")})")
        Next
        cmbBarang.SelectedIndex = 0
    End Sub

    Private Sub LoadData()
        dgvMasuk.DataSource = DatabaseHelper.GetDataTable(
            "SELECT bm.MasukID AS 'ID', bm.NoTransaksi AS 'No. Transaksi', " &
            "b.KodeBarang AS 'Kode', b.NamaBarang AS 'Nama Barang', " &
            "bm.Jumlah, bm.HargaBeli AS 'Harga Beli', bm.Supplier, " &
            "bm.TanggalMasuk AS 'Tanggal', COALESCE(bm.Keterangan,'') AS 'Keterangan' " &
            "FROM BarangMasuk bm JOIN Barang b ON bm.BarangID = b.BarangID " &
            "ORDER BY bm.TanggalMasuk DESC, bm.MasukID DESC")
        If dgvMasuk.Columns.Contains("ID") Then dgvMasuk.Columns("ID").Visible = False
    End Sub

    Private Sub GenerateNo()
        txtNoTransaksi.Text = DatabaseHelper.GenerateTransactionNumber("BM")
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs)
        If cmbBarang.SelectedIndex <= 0 Then
            MessageBox.Show("Pilih barang terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim barangId As Integer = Integer.Parse(cmbBarang.SelectedItem.ToString().Split("|"c)(0))
        Dim jumlah As Integer = CInt(nudJumlah.Value)

        Try
            Dim params As New Dictionary(Of String, Object) From {
                {"@no", txtNoTransaksi.Text}, {"@barang", barangId},
                {"@jumlah", jumlah}, {"@harga", nudHargaBeli.Value},
                {"@supplier", txtSupplier.Text.Trim()},
                {"@tanggal", dtpTanggal.Value.ToString("yyyy-MM-dd")},
                {"@ket", txtKeterangan.Text.Trim()}
            }
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO BarangMasuk (NoTransaksi,BarangID,Jumlah,HargaBeli,Supplier,TanggalMasuk,Keterangan) " &
                "VALUES (@no,@barang,@jumlah,@harga,@supplier,@tanggal,@ket)", params)

            ' Update stok
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE Barang SET StokSaatIni = StokSaatIni + @jumlah, UpdatedAt = CURRENT_TIMESTAMP WHERE BarangID = @id",
                New Dictionary(Of String, Object) From {{"@jumlah", jumlah}, {"@id", barangId}})

            MessageBox.Show("Barang masuk berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadData()
            GenerateNo()
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs)
        If dgvMasuk.SelectedRows.Count = 0 Then
            MessageBox.Show("Pilih transaksi yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Yakin ingin menghapus transaksi ini? Stok akan dikurangi kembali.", "Konfirmasi",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim row = dgvMasuk.SelectedRows(0)
            Dim masukId As Integer = Convert.ToInt32(row.Cells("ID").Value)

            ' Get barangID and jumlah for stock revert
            Dim dt = DatabaseHelper.GetDataTable("SELECT BarangID, Jumlah FROM BarangMasuk WHERE MasukID = @id",
                New Dictionary(Of String, Object) From {{"@id", masukId}})

            If dt.Rows.Count > 0 Then
                Dim barangId As Integer = Convert.ToInt32(dt.Rows(0)("BarangID"))
                Dim jumlah As Integer = Convert.ToInt32(dt.Rows(0)("Jumlah"))

                DatabaseHelper.ExecuteNonQuery("DELETE FROM BarangMasuk WHERE MasukID = @id",
                    New Dictionary(Of String, Object) From {{"@id", masukId}})
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE Barang SET StokSaatIni = MAX(0, StokSaatIni - @jumlah), UpdatedAt = CURRENT_TIMESTAMP WHERE BarangID = @bid",
                    New Dictionary(Of String, Object) From {{"@jumlah", jumlah}, {"@bid", barangId}})

                MessageBox.Show("Transaksi berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadData()
            End If
        End If
    End Sub

    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ClearFields()
        cmbBarang.SelectedIndex = 0
        nudJumlah.Value = 1
        nudHargaBeli.Value = 0
        txtSupplier.Clear()
        txtKeterangan.Clear()
        dtpTanggal.Value = DateTime.Now
    End Sub

End Class
