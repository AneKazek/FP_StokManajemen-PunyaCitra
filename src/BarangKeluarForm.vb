Imports System.Windows.Forms
Imports System.Drawing

Public Class BarangKeluarForm
    Inherits Form

    Private dgvKeluar As DataGridView
    Private txtNoTransaksi, txtTujuan, txtKeterangan As TextBox
    Private lblStokInfo As Label
    Private cmbBarang As ComboBox
    Private nudJumlah As NumericUpDown
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
        pnl.Size = New Size(980, 240)
        pnl.BackColor = ModernUI.ColorCard
        pnl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim title As Label = ModernUI.CreateSectionLabel("▲ Form Barang Keluar")
        title.Location = New Point(20, 15)
        pnl.Controls.Add(title)

        pnl.Controls.Add(FL("No. Transaksi:", 20, 53))
        txtNoTransaksi = New TextBox()
        txtNoTransaksi.Location = New Point(150, 50) : txtNoTransaksi.Size = New Size(200, 30) : txtNoTransaksi.ReadOnly = True
        ModernUI.StyleTextBox(txtNoTransaksi) : pnl.Controls.Add(txtNoTransaksi)

        pnl.Controls.Add(FL("Tanggal:", 380, 53))
        dtpTanggal = New DateTimePicker()
        dtpTanggal.Location = New Point(460, 50) : dtpTanggal.Size = New Size(180, 30)
        ModernUI.StyleDatePicker(dtpTanggal) : pnl.Controls.Add(dtpTanggal)

        pnl.Controls.Add(FL("Barang:", 20, 93))
        cmbBarang = New ComboBox()
        cmbBarang.Location = New Point(150, 90) : cmbBarang.Size = New Size(350, 30)
        ModernUI.StyleComboBox(cmbBarang)
        AddHandler cmbBarang.SelectedIndexChanged, AddressOf CmbBarang_Changed
        pnl.Controls.Add(cmbBarang)

        lblStokInfo = New Label()
        lblStokInfo.Text = "Stok: -" : lblStokInfo.Font = New Font("Segoe UI Semibold", 10)
        lblStokInfo.ForeColor = ModernUI.ColorPrimary : lblStokInfo.Location = New Point(520, 93) : lblStokInfo.AutoSize = True
        pnl.Controls.Add(lblStokInfo)

        pnl.Controls.Add(FL("Jumlah:", 700, 93))
        nudJumlah = New NumericUpDown()
        nudJumlah.Location = New Point(770, 90) : nudJumlah.Size = New Size(100, 30)
        nudJumlah.Minimum = 1 : nudJumlah.Maximum = 999999
        ModernUI.StyleNumericUpDown(nudJumlah) : pnl.Controls.Add(nudJumlah)

        pnl.Controls.Add(FL("Tujuan:", 20, 133))
        txtTujuan = New TextBox()
        txtTujuan.Location = New Point(150, 130) : txtTujuan.Size = New Size(250, 30)
        ModernUI.StyleTextBox(txtTujuan) : pnl.Controls.Add(txtTujuan)

        pnl.Controls.Add(FL("Keterangan:", 420, 133))
        txtKeterangan = New TextBox()
        txtKeterangan.Location = New Point(530, 130) : txtKeterangan.Size = New Size(420, 30)
        ModernUI.StyleTextBox(txtKeterangan) : pnl.Controls.Add(txtKeterangan)

        btnSimpan = New Button() : btnSimpan.Text = "Simpan Transaksi" : btnSimpan.Location = New Point(20, 175)
        ModernUI.StyleButton(btnSimpan, ModernUI.ColorSuccess, 180) : AddHandler btnSimpan.Click, AddressOf BtnSimpan_Click
        pnl.Controls.Add(btnSimpan)

        btnBatal = New Button() : btnBatal.Text = "Batal" : btnBatal.Location = New Point(210, 175)
        ModernUI.StyleButton(btnBatal, Color.FromArgb(149, 165, 166)) : AddHandler btnBatal.Click, AddressOf BtnBatal_Click
        pnl.Controls.Add(btnBatal)

        Me.Controls.Add(pnl)
    End Sub

    Private Function FL(t As String, x As Integer, y As Integer) As Label
        Dim l As Label = ModernUI.CreateFieldLabel(t) : l.Location = New Point(x, y) : Return l
    End Function

    Private Sub SetupGrid()
        Dim lbl As Label = ModernUI.CreateSectionLabel("Riwayat Barang Keluar")
        lbl.Location = New Point(25, 280) : Me.Controls.Add(lbl)

        btnHapus = New Button() : btnHapus.Text = "Hapus Transaksi" : btnHapus.Location = New Point(830, 275)
        ModernUI.StyleButton(btnHapus, ModernUI.ColorDanger, 170) : AddHandler btnHapus.Click, AddressOf BtnHapus_Click
        Me.Controls.Add(btnHapus)

        dgvKeluar = New DataGridView()
        dgvKeluar.Location = New Point(25, 310) : dgvKeluar.Size = New Size(980, 350)
        dgvKeluar.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ModernUI.StyleDataGridView(dgvKeluar) : Me.Controls.Add(dgvKeluar)
    End Sub

    Private Sub LoadBarang()
        cmbBarang.Items.Clear() : cmbBarang.Items.Add("-- Pilih Barang --")
        Dim dt = DatabaseHelper.GetDataTable("SELECT BarangID,KodeBarang,NamaBarang,Satuan,StokSaatIni FROM Barang ORDER BY NamaBarang")
        For Each row As Data.DataRow In dt.Rows
            cmbBarang.Items.Add($"{row("BarangID")}|{row("KodeBarang")} - {row("NamaBarang")} (Stok:{row("StokSaatIni")} {row("Satuan")})")
        Next
        cmbBarang.SelectedIndex = 0
    End Sub

    Private Sub CmbBarang_Changed(sender As Object, e As EventArgs)
        If cmbBarang.SelectedIndex <= 0 Then lblStokInfo.Text = "Stok: -" : Return
        Dim bid As Integer = Integer.Parse(cmbBarang.SelectedItem.ToString().Split("|"c)(0))
        Dim stok As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT StokSaatIni FROM Barang WHERE BarangID=@id", New Dictionary(Of String, Object) From {{"@id", bid}}))
        lblStokInfo.Text = $"Stok tersedia: {stok}"
        lblStokInfo.ForeColor = If(stok > 0, ModernUI.ColorSuccess, ModernUI.ColorDanger)
    End Sub

    Private Sub LoadData()
        dgvKeluar.DataSource = DatabaseHelper.GetDataTable(
            "SELECT bk.KeluarID AS 'ID',bk.NoTransaksi AS 'No. Transaksi',b.KodeBarang AS 'Kode',b.NamaBarang AS 'Nama Barang'," &
            "bk.Jumlah,bk.Tujuan,bk.TanggalKeluar AS 'Tanggal',COALESCE(bk.Keterangan,'') AS 'Keterangan' " &
            "FROM BarangKeluar bk JOIN Barang b ON bk.BarangID=b.BarangID ORDER BY bk.TanggalKeluar DESC,bk.KeluarID DESC")
        If dgvKeluar.Columns.Contains("ID") Then dgvKeluar.Columns("ID").Visible = False
    End Sub

    Private Sub GenerateNo()
        txtNoTransaksi.Text = DatabaseHelper.GenerateTransactionNumber("BK")
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs)
        If cmbBarang.SelectedIndex <= 0 Then MessageBox.Show("Pilih barang!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        Dim bid As Integer = Integer.Parse(cmbBarang.SelectedItem.ToString().Split("|"c)(0))
        Dim jml As Integer = CInt(nudJumlah.Value)
        Dim curStok As Integer = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT StokSaatIni FROM Barang WHERE BarangID=@id", New Dictionary(Of String, Object) From {{"@id", bid}}))
        If jml > curStok Then MessageBox.Show($"Stok tidak mencukupi! Tersedia: {curStok}", "Stok Kurang", MessageBoxButtons.OK, MessageBoxIcon.Error) : Return
        If String.IsNullOrWhiteSpace(txtTujuan.Text) Then MessageBox.Show("Tujuan harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        Try
            DatabaseHelper.ExecuteNonQuery("INSERT INTO BarangKeluar(NoTransaksi,BarangID,Jumlah,Tujuan,TanggalKeluar,Keterangan) VALUES(@no,@b,@j,@t,@tgl,@k)",
                New Dictionary(Of String, Object) From {{"@no", txtNoTransaksi.Text}, {"@b", bid}, {"@j", jml}, {"@t", txtTujuan.Text.Trim()}, {"@tgl", dtpTanggal.Value.ToString("yyyy-MM-dd")}, {"@k", txtKeterangan.Text.Trim()}})
            DatabaseHelper.ExecuteNonQuery("UPDATE Barang SET StokSaatIni=StokSaatIni-@j,UpdatedAt=CURRENT_TIMESTAMP WHERE BarangID=@id",
                New Dictionary(Of String, Object) From {{"@j", jml}, {"@id", bid}})
            MessageBox.Show("Barang keluar berhasil dicatat!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields() : LoadBarang() : LoadData() : GenerateNo()
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs)
        If dgvKeluar.SelectedRows.Count = 0 Then MessageBox.Show("Pilih transaksi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning) : Return
        If MessageBox.Show("Yakin hapus? Stok dikembalikan.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim kid As Integer = Convert.ToInt32(dgvKeluar.SelectedRows(0).Cells("ID").Value)
            Dim dt = DatabaseHelper.GetDataTable("SELECT BarangID,Jumlah FROM BarangKeluar WHERE KeluarID=@id", New Dictionary(Of String, Object) From {{"@id", kid}})
            If dt.Rows.Count > 0 Then
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BarangKeluar WHERE KeluarID=@id", New Dictionary(Of String, Object) From {{"@id", kid}})
                DatabaseHelper.ExecuteNonQuery("UPDATE Barang SET StokSaatIni=StokSaatIni+@j,UpdatedAt=CURRENT_TIMESTAMP WHERE BarangID=@bid",
                    New Dictionary(Of String, Object) From {{"@j", Convert.ToInt32(dt.Rows(0)("Jumlah"))}, {"@bid", Convert.ToInt32(dt.Rows(0)("BarangID"))}})
                MessageBox.Show("Transaksi dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBarang() : LoadData()
            End If
        End If
    End Sub

    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ClearFields()
        cmbBarang.SelectedIndex = 0 : nudJumlah.Value = 1
        txtTujuan.Clear() : txtKeterangan.Clear() : dtpTanggal.Value = DateTime.Now : lblStokInfo.Text = "Stok: -"
    End Sub
End Class
