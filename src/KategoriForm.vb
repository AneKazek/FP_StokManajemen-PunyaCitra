Imports System.Windows.Forms
Imports System.Drawing

Public Class KategoriForm
    Inherits Form

    Private dgvKategori As DataGridView
    Private txtNama As TextBox
    Private txtDeskripsi As TextBox
    Private btnSimpan As Button
    Private btnEdit As Button
    Private btnHapus As Button
    Private btnBatal As Button
    Private selectedId As Integer = -1

    Public Sub New()
        SetupForm()
        SetupInputPanel()
        SetupGrid()
        LoadData()
    End Sub

    Private Sub SetupForm()
        Me.BackColor = ModernUI.ColorBackground
        Me.AutoScroll = True
        Me.Padding = New Padding(25)
    End Sub

    Private Sub SetupInputPanel()
        Dim pnl As New Panel()
        pnl.Location = New Point(25, 25)
        pnl.Size = New Size(980, 150)
        pnl.BackColor = ModernUI.ColorCard
        pnl.Padding = New Padding(20)

        Dim lblTitle As Label = ModernUI.CreateSectionLabel("Data Kategori")
        lblTitle.Location = New Point(20, 15)
        pnl.Controls.Add(lblTitle)

        ' Nama Kategori
        Dim lblNama As Label = ModernUI.CreateFieldLabel("Nama Kategori:")
        lblNama.Location = New Point(20, 50)
        pnl.Controls.Add(lblNama)

        txtNama = New TextBox()
        txtNama.Location = New Point(150, 47)
        txtNama.Size = New Size(300, 30)
        ModernUI.StyleTextBox(txtNama)
        pnl.Controls.Add(txtNama)

        ' Deskripsi
        Dim lblDesk As Label = ModernUI.CreateFieldLabel("Deskripsi:")
        lblDesk.Location = New Point(20, 85)
        pnl.Controls.Add(lblDesk)

        txtDeskripsi = New TextBox()
        txtDeskripsi.Location = New Point(150, 82)
        txtDeskripsi.Size = New Size(300, 30)
        ModernUI.StyleTextBox(txtDeskripsi)
        pnl.Controls.Add(txtDeskripsi)

        ' Buttons
        btnSimpan = New Button()
        btnSimpan.Text = "💾 Simpan"
        btnSimpan.Location = New Point(500, 47)
        ModernUI.StyleButton(btnSimpan, ModernUI.ColorPrimary)
        AddHandler btnSimpan.Click, AddressOf BtnSimpan_Click
        pnl.Controls.Add(btnSimpan)

        btnEdit = New Button()
        btnEdit.Text = "✏ Update"
        btnEdit.Location = New Point(640, 47)
        ModernUI.StyleButton(btnEdit, ModernUI.ColorWarning)
        btnEdit.Enabled = False
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        pnl.Controls.Add(btnEdit)

        btnHapus = New Button()
        btnHapus.Text = "🗑 Hapus"
        btnHapus.Location = New Point(500, 90)
        ModernUI.StyleButton(btnHapus, ModernUI.ColorDanger)
        btnHapus.Enabled = False
        AddHandler btnHapus.Click, AddressOf BtnHapus_Click
        pnl.Controls.Add(btnHapus)

        btnBatal = New Button()
        btnBatal.Text = "✖ Batal"
        btnBatal.Location = New Point(640, 90)
        ModernUI.StyleButton(btnBatal, Color.FromArgb(149, 165, 166))
        AddHandler btnBatal.Click, AddressOf BtnBatal_Click
        pnl.Controls.Add(btnBatal)

        Me.Controls.Add(pnl)
    End Sub

    Private Sub SetupGrid()
        dgvKategori = New DataGridView()
        dgvKategori.Location = New Point(25, 195)
        dgvKategori.Size = New Size(980, 400)
        dgvKategori.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        ModernUI.StyleDataGridView(dgvKategori)
        AddHandler dgvKategori.CellClick, AddressOf DgvKategori_CellClick
        Me.Controls.Add(dgvKategori)
    End Sub

    Private Sub LoadData()
        Dim dt = DatabaseHelper.GetDataTable(
            "SELECT KategoriID AS 'ID', NamaKategori AS 'Nama Kategori', " &
            "COALESCE(Deskripsi,'') AS 'Deskripsi', CreatedAt AS 'Tanggal Dibuat' FROM Kategori ORDER BY NamaKategori")
        dgvKategori.DataSource = dt
        If dgvKategori.Columns.Contains("ID") Then dgvKategori.Columns("ID").Visible = False
    End Sub

    Private Sub DgvKategori_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row = dgvKategori.Rows(e.RowIndex)
        selectedId = Convert.ToInt32(row.Cells("ID").Value)
        txtNama.Text = row.Cells("Nama Kategori").Value.ToString()
        txtDeskripsi.Text = row.Cells("Deskripsi").Value.ToString()
        btnEdit.Enabled = True
        btnHapus.Enabled = True
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNama.Text) Then
            MessageBox.Show("Nama Kategori harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim params As New Dictionary(Of String, Object) From {
            {"@nama", txtNama.Text.Trim()},
            {"@desk", txtDeskripsi.Text.Trim()}
        }
        DatabaseHelper.ExecuteNonQuery("INSERT INTO Kategori (NamaKategori, Deskripsi) VALUES (@nama, @desk)", params)
        MessageBox.Show("Kategori berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ClearFields()
        LoadData()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        If selectedId < 0 OrElse String.IsNullOrWhiteSpace(txtNama.Text) Then Return
        Dim params As New Dictionary(Of String, Object) From {
            {"@nama", txtNama.Text.Trim()},
            {"@desk", txtDeskripsi.Text.Trim()},
            {"@id", selectedId}
        }
        DatabaseHelper.ExecuteNonQuery("UPDATE Kategori SET NamaKategori = @nama, Deskripsi = @desk WHERE KategoriID = @id", params)
        MessageBox.Show("Kategori berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ClearFields()
        LoadData()
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs)
        If selectedId < 0 Then Return
        If MessageBox.Show("Yakin ingin menghapus kategori ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            DatabaseHelper.ExecuteNonQuery("DELETE FROM Kategori WHERE KategoriID = @id", New Dictionary(Of String, Object) From {{"@id", selectedId}})
            MessageBox.Show("Kategori berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadData()
        End If
    End Sub

    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtNama.Clear()
        txtDeskripsi.Clear()
        selectedId = -1
        btnEdit.Enabled = False
        btnHapus.Enabled = False
    End Sub

End Class
