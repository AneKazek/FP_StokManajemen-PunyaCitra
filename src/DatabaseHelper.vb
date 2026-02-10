Imports Microsoft.Data.Sqlite
Imports System.IO
Imports System.Data
Imports System.Windows.Forms

Public Class DatabaseHelper

    Private Shared ReadOnly DbFileName As String = "stokbarang.db"

    Public Shared Function GetConnectionString() As String
        Dim dbPath As String = Path.Combine(Application.StartupPath, DbFileName)
        Return $"Data Source={dbPath}"
    End Function

    Public Shared Function GetConnection() As SqliteConnection
        Return New SqliteConnection(GetConnectionString())
    End Function

    Public Shared Sub InitializeDatabase()
        Using conn = GetConnection()
            conn.Open()

            Dim sql As String = "
                CREATE TABLE IF NOT EXISTS Kategori (
                    KategoriID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NamaKategori TEXT NOT NULL,
                    Deskripsi TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS Barang (
                    BarangID INTEGER PRIMARY KEY AUTOINCREMENT,
                    KodeBarang TEXT UNIQUE NOT NULL,
                    NamaBarang TEXT NOT NULL,
                    KategoriID INTEGER,
                    Satuan TEXT NOT NULL,
                    HargaBeli REAL DEFAULT 0,
                    HargaJual REAL DEFAULT 0,
                    StokMinimum INTEGER DEFAULT 0,
                    StokSaatIni INTEGER DEFAULT 0,
                    Lokasi TEXT,
                    Keterangan TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (KategoriID) REFERENCES Kategori(KategoriID)
                );

                CREATE TABLE IF NOT EXISTS BarangMasuk (
                    MasukID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NoTransaksi TEXT UNIQUE NOT NULL,
                    BarangID INTEGER NOT NULL,
                    Jumlah INTEGER NOT NULL,
                    HargaBeli REAL DEFAULT 0,
                    Supplier TEXT,
                    TanggalMasuk DATETIME NOT NULL,
                    Keterangan TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (BarangID) REFERENCES Barang(BarangID)
                );

                CREATE TABLE IF NOT EXISTS BarangKeluar (
                    KeluarID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NoTransaksi TEXT UNIQUE NOT NULL,
                    BarangID INTEGER NOT NULL,
                    Jumlah INTEGER NOT NULL,
                    Tujuan TEXT,
                    TanggalKeluar DATETIME NOT NULL,
                    Keterangan TEXT,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (BarangID) REFERENCES Barang(BarangID)
                );"

            Using cmd = conn.CreateCommand()
                cmd.CommandText = sql
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Shared Function GetDataTable(sql As String, Optional params As Dictionary(Of String, Object) = Nothing) As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection()
            conn.Open()
            Using cmd = conn.CreateCommand()
                cmd.CommandText = sql
                AddParameters(cmd, params)
                Using reader = cmd.ExecuteReader()
                    dt.Load(reader)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Shared Function ExecuteNonQuery(sql As String, Optional params As Dictionary(Of String, Object) = Nothing) As Integer
        Using conn = GetConnection()
            conn.Open()
            Using cmd = conn.CreateCommand()
                cmd.CommandText = sql
                AddParameters(cmd, params)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteScalar(sql As String, Optional params As Dictionary(Of String, Object) = Nothing) As Object
        Using conn = GetConnection()
            conn.Open()
            Using cmd = conn.CreateCommand()
                cmd.CommandText = sql
                AddParameters(cmd, params)
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

    Private Shared Sub AddParameters(cmd As SqliteCommand, params As Dictionary(Of String, Object))
        If params IsNot Nothing Then
            For Each kvp In params
                cmd.Parameters.AddWithValue(kvp.Key, If(kvp.Value, DBNull.Value))
            Next
        End If
    End Sub

    Public Shared Function GenerateTransactionNumber(prefix As String) As String
        Dim today As String = DateTime.Now.ToString("yyyyMMdd")
        Dim pattern As String = $"{prefix}-{today}-%"
        Dim tableName As String = If(prefix = "BM", "BarangMasuk", "BarangKeluar")
        Dim sql As String = $"SELECT COUNT(*) FROM {tableName} WHERE NoTransaksi LIKE @pattern"
        Dim params As New Dictionary(Of String, Object) From {{"@pattern", pattern}}
        Dim count As Integer = Convert.ToInt32(ExecuteScalar(sql, params))
        Return $"{prefix}-{today}-{(count + 1).ToString("D3")}"
    End Function

End Class
