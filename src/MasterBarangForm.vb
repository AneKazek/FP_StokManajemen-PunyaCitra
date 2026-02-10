Imports System.Windows.Forms
Imports System.Drawing

Public Class MasterBarangForm
    Inherits Form

    Private dgvBarang As DataGridView
    Private txtKode, txtNama, txtSatuan, txtLokasi, txtKeterangan, txtSearch As TextBox
    Private cmbKategori As ComboBox
    Private nudHargaBeli, nudHargaJual, nudStokMin As NumericUpDown
    Private btnSimpan, btnEdit, btnHapus, btnBatal As Button
    Private selectedId As Integer = -1

    Public Sub New()
        Me.BackColor = ModernUI.ColorBackground
        Me.AutoScroll = True
        Me.Padding = New Padding(25)
        SetupInputPanel()
        SetupGridPanel()
        LoadKategori()
        LoadData()
    End Sub

    Private Sub SetupInputPanel()
        Dim pnl As New Panel()
        pnl.Location = New Point(25, 25)
        pnl.Size = New Size(980, 280)
        pnl.BackColor = ModernUI.ColorCard
        pnl.Padding = New Padding(20)
        pnl.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim title As Label = ModernUI.CreateSectionLabel("Form Data Barang")
        title.Location = New Point(20, 15)
        pnl.Controls.Add(title)

        ' Row 1
        AddField(pnl, "Kode Barang:", 20, 50, txtKode, 180)
        AddField(pnl, "Nama Barang:", 20, 85, txtNama, 300)
        AddField(pnl, "Satuan:", 500, 50, txtSatuan, 120)

        ' Kategori
        Dim lblKat As Label = ModernUI.CreateFieldLabel("Kategori:")
        lblKat.Location = New Point(500, 88)
        pnl.Controls.Add(lblKat)
        cmbKategori = New ComboBox()
        cmbKategori.Location = New Point(590, 85)
        cmbKategori.Size = New Size(200, 30)
        ModernUI.StyleComboBox(cmbKategori)
        pnl.Controls.Add(cmbKategori)

        ' Row 2 - Numeric fields
        Dim lblHB As Label = ModernUI.CreateFieldLabel("Harga Beli:")
        lblHB.Location = New Point(20, 123)
        pnl.Controls.Add(lblHB)
        nudHargaBeli = New NumericUpDown()
        nudHargaBeli.Location = New Point(130, 120)
        nudHargaBeli.Size = New Size(150, 30)
        nudHargaBeli.Maximum = 999999999
        nudHargaBeli.DecimalPlaces = 0
        nudHargaBeli.ThousandsSeparator = True
        ModernUI.StyleNumericUpDown(nudHargaBeli)
        pnl.Controls.Add(nudHargaBeli)

        Dim lblHJ As Label = ModernUI.CreateFieldLabel("Harga Jual:")
        lblHJ.Location = New Point(300, 123)
        pnl.Controls.Add(lblHJ)
        nudHargaJual = New NumericUpDown()
        nudHargaJual.Location = New Point(400, 120)
        nudHargaJual.Size = New Size(150, 30)
        nudHargaJual.Maximum = 999999999
        nudHargaJual.DecimalPlaces = 0
        nudHargaJual.ThousandsSeparator = True
        ModernUI.StyleNumericUpDown(nudHargaJual)
        pnl.Controls.Add(nudHargaJual)

        Dim lblSM As Label = ModernUI.CreateFieldLabel("Stok Min:")
        lblSM.Location = New Point(570, 123)
        pnl.Controls.Add(lblSM)
        nudStokMin = New NumericUpDown()
        nudStokMin.Location = New Point(650, 120)
        nudStokMin.Size = New Size(100, 30)
        nudStokMin.Maximum = 999999
        ModernUI.StyleNumericUpDown(nudStokMin)
        pnl.Controls.Add(nudStokMin)

        ' Row 3
        AddField(pnl, "Lokasi:", 20, 160, txtLokasi, 200)
        AddField(pnl, "Keterangan:", 350, 160, txtKeterangan, 300)

        ' Buttons
        btnSimpan = New Button() : btnSimpan.Text = "💾 Simpan"
        btnSimpan.Location = New Point(20, 200)
        ModernUI.StyleButton(btnSimpan, ModernUI.ColorPrimary)
        AddHandler btnSimpan.Click, AddressOf BtnSimpan_Click
        pnl.Controls.Add(btnSimpan)

        btnEdit = New Button() : btnEdit.Text = "✏ Update"
        btnEdit.Location = New Point(160, 200) : btnEdit.Enabled = False
        ModernUI.StyleButton(btnEdit, ModernUI.ColorWarning)
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        pnl.Controls.Add(btnEdit)

        btnHapus = New Button() : btnHapus.Text = "🗑 Hapus"
        btnHapus.Location = New Point(300, 200) : btnHapus.Enabled = False
        ModernUI.StyleButton(btnHapus, ModernUI.ColorDanger)
        AddHandler btnHapus.Click, AddressOf BtnHapus_Click
        pnl.Controls.Add(btnHapus)

        btnBatal = New Button() : btnBatal.Text = "✖ Batal"
        btnBatal.Location = New Point(440, 200)
        ModernUI.StyleButton(btnBatal, Color.FromArgb(149, 165, 166))
        AddHandler btnBatal.Click, AddressOf BtnBatal_Click
        pnl.Controls.Add(btnBatal)

        Me.Controls.Add(pnl)
    End Sub

    Private Sub AddField(parent As Panel, labelText As String, x As Integer, y As Integer, ByRef txt As TextBox, txtWidth As Integer)
        Dim lbl As Label = ModernUI.CreateFieldLabel(labelText)
        lbl.Location = New Point(x, y + 3)
        parent.Controls.Add(lbl)

        txt = New TextBox()
        txt.Location = New Point(x + lbl.PreferredWidth + 10, y)
        txt.Size = New Size(txtWidth, 30)
        ModernUI.StyleTextBox(txt)
        parent.Controls.Add(txt)
    End Sub

    Private Sub SetupGridPanel()
        ' Search
        Dim lblSearch As Label = ModernUI.CreateFieldLabel("🔍 Cari:")
        lblSearch.Location = New Point(25, 320)
        Me.Controls.Add(lblSearch)

        txtSearch = New TextBox()
        txtSearch.Location = New Point(80, 317)
        txtSearch.Size = New Size(300, 30)
        ModernUI.StyleTextBox(txtSearch)
        AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged
        Me.Controls.Add(txtSearch)

        ' Grid
        dgvBarang = New DataGridView()
        dgvBarang.Location = New Point(25, 355)
        dgvBarang.Size = New Size(980, 350)
        dgvBarang.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ModernUI.StyleDataGridView(dgvBarang)
        AddHandler dgvBarang.CellClick, AddressOf DgvBarang_CellClick
        Me.Controls.Add(dgvBarang)
    End Sub

    Private Sub LoadKategori()
        cmbKategori.Items.Clear()
        cmbKategori.Items.Add("-- Pilih Kategori --")
        Dim dt = DatabaseHelper.GetDataTable("SELECT KategoriID, NamaKategori FROM Kategori ORDER BY NamaKategori")
        For Each row As Data.DataRow In dt.Rows
            cmbKategori.Items.Add($"{row("KategoriID")}|{row("NamaKategori")}")
        Next
        cmbKategori.SelectedIndex = 0
    End Sub

    Private Sub LoadData(Optional search As String = "")
        Dim sql As String = "SELECT b.BarangID AS 'ID', b.KodeBarang AS 'Kode', b.NamaBarang AS 'Nama Barang', " &
            "COALESCE(k.NamaKategori,'') AS 'Kategori', b.Satuan, b.HargaBeli AS 'Harga Beli', " &
            "b.HargaJual AS 'Harga Jual', b.StokMinimum AS 'Stok Min', b.StokSaatIni AS 'Stok', " &
            "COALESCE(b.Lokasi,'') AS 'Lokasi' " &
            "FROM Barang b LEFT JOIN Kategori k ON b.KategoriID = k.KategoriID "

        Dim params As New Dictionary(Of String, Object)
        If Not String.IsNullOrEmpty(search) Then
            sql &= "WHERE b.NamaBarang LIKE @s OR b.KodeBarang LIKE @s "
            params.Add("@s", $"%{search}%")
        End If
        sql &= "ORDER BY b.NamaBarang"

        dgvBarang.DataSource = DatabaseHelper.GetDataTable(sql, params)
        If dgvBarang.Columns.Contains("ID") Then dgvBarang.Columns("ID").Visible = False
    End Sub

    Private Sub DgvBarang_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row = dgvBarang.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells("ID").Value)
        txtKode.Text = row.Cells("Kode").Value.ToString()
        txtNama.Text = row.Cells("Nama Barang").Value.ToString()
        txtSatuan.Text = row.Cells("Satuan").Value.ToString()
        nudHargaBeli.Value = Convert.ToDecimal(row.Cells("Harga Beli").Value)
        nudHargaJual.Value = Convert.ToDecimal(row.Cells("Harga Jual").Value)
        nudStokMin.Value = Convert.ToDecimal(row.Cells("Stok Min").Value)
        txtLokasi.Text = row.Cells("Lokasi").Value.ToString()

        ' Select matching kategori
        Dim kat As String = row.Cells("Kategori").Value.ToString()
        For i As Integer = 0 To cmbKategori.Items.Count - 1
            If cmbKategori.Items(i).ToString().EndsWith(kat) Then
                cmbKategori.SelectedIndex = i
                Exit For
            End If
        Next

        btnEdit.Enabled = True
        btnHapus.Enabled = True
    End Sub

    Private Function GetKategoriId() As Object
        If cmbKategori.SelectedIndex <= 0 Then Return DBNull.Value
        Return Integer.Parse(cmbKategori.SelectedItem.ToString().Split("|"c)(0))
    End Function

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtKode.Text) OrElse String.IsNullOrWhiteSpace(txtNama.Text) OrElse String.IsNullOrWhiteSpace(txtSatuan.Text) Then
            MessageBox.Show("Kode, Nama, dan Satuan harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim params As New Dictionary(Of String, Object) From {
                {"@kode", txtKode.Text.Trim()}, {"@nama", txtNama.Text.Trim()},
                {"@kat", GetKategoriId()}, {"@satuan", txtSatuan.Text.Trim()},
                {"@hb", nudHargaBeli.Value}, {"@hj", nudHargaJual.Value},
                {"@sm", CInt(nudStokMin.Value)}, {"@lok", txtLokasi.Text.Trim()},
                {"@ket", txtKeterangan.Text.Trim()}
            }
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO Barang (KodeBarang,NamaBarang,KategoriID,Satuan,HargaBeli,HargaJual,StokMinimum,Lokasi,Keterangan) " &
                "VALUES (@kode,@nama,@kat,@satuan,@hb,@hj,@sm,@lok,@ket)", params)
            MessageBox.Show("Barang berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadData()
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        If selectedId < 0 Then Return
        Try
            Dim params As New Dictionary(Of String, Object) From {
                {"@kode", txtKode.Text.Trim()}, {"@nama", txtNama.Text.Trim()},
                {"@kat", GetKategoriId()}, {"@satuan", txtSatuan.Text.Trim()},
                {"@hb", nudHargaBeli.Value}, {"@hj", nudHargaJual.Value},
                {"@sm", CInt(nudStokMin.Value)}, {"@lok", txtLokasi.Text.Trim()},
                {"@ket", txtKeterangan.Text.Trim()}, {"@id", selectedId}
            }
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE Barang SET KodeBarang=@kode,NamaBarang=@nama,KategoriID=@kat,Satuan=@satuan," &
                "HargaBeli=@hb,HargaJual=@hj,StokMinimum=@sm,Lokasi=@lok,Keterangan=@ket," &
                "UpdatedAt=CURRENT_TIMESTAMP WHERE BarangID=@id", params)
            MessageBox.Show("Barang berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadData()
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs)
        If selectedId < 0 Then Return
        If MessageBox.Show("Yakin ingin menghapus barang ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            DatabaseHelper.ExecuteNonQuery("DELETE FROM Barang WHERE BarangID = @id", New Dictionary(Of String, Object) From {{"@id", selectedId}})
            MessageBox.Show("Barang berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadData()
        End If
    End Sub

    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadData(txtSearch.Text)
    End Sub

    Private Sub ClearFields()
        txtKode.Clear() : txtNama.Clear() : txtSatuan.Clear() : txtLokasi.Clear() : txtKeterangan.Clear()
        nudHargaBeli.Value = 0 : nudHargaJual.Value = 0 : nudStokMin.Value = 0
        If cmbKategori.Items.Count > 0 Then cmbKategori.SelectedIndex = 0
        selectedId = -1 : btnEdit.Enabled = False : btnHapus.Enabled = False
    End Sub

End Class
