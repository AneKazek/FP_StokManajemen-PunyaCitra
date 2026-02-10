-- Tabel Kategori
CREATE TABLE Kategori (
    KategoriID INTEGER PRIMARY KEY AUTOINCREMENT,
    NamaKategori TEXT NOT NULL,
    Deskripsi TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabel Barang (Master)
CREATE TABLE Barang (
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

-- Tabel Barang Masuk
CREATE TABLE BarangMasuk (
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

-- Tabel Barang Keluar
CREATE TABLE BarangKeluar (
    KeluarID INTEGER PRIMARY KEY AUTOINCREMENT,
    NoTransaksi TEXT UNIQUE NOT NULL,
    BarangID INTEGER NOT NULL,
    Jumlah INTEGER NOT NULL,
    Tujuan TEXT,
    TanggalKeluar DATETIME NOT NULL,
    Keterangan TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (BarangID) REFERENCES Barang(BarangID)
);
