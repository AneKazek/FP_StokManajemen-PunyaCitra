# 🛠️ Panduan Teknis (Developer Guide)

Dokumen ini diperuntukkan bagi pengembang atau technical support yang ingin memodifikasi atau memahami struktur dalam aplikasi **Stok Barang App**.

## Arsitektur Aplikasi

-   **Bahasa Pemrograman**: Visual Basic .NET (VB.NET)
-   **Framework**: .NET 8.0 (Windows Forms)
-   **Database**: SQLite (`stokbarang.db`)
    -   *Portable*, file database otomatis dibuat di folder `bin/Debug/net8.0-windows/` saat pertama run.

## Struktur Project

Folder `src/` berisi source code utama:

-   `Program.vb`: Entry point aplikasi. Mengatur Application Configuration dan menjalankan `MainForm`.
-   `DatabaseHelper.vb`:
    -   Singleton-pattern connection ke SQLite.
    -   Method CRUD generic (`ExecuteNonQuery`, `ExecuteQuery`).
    -   `InitializeDatabase()`: Membuat tabel otomatis jika belum ada.
-   `ModernUI.vb`:
    -   Module styling terpusat. Mengatur `ColorPalette` (Navy Blue Theme).
    -   Helper functions: `StyleButton`, `StyleDataGridView`, `CreateCard`.
    -   Memudahkan penggantian tema warna secara global.

### Form (UI)

-   `MainForm.vb`: Parent container. Menggunakan `Panel` untuk layout (Sidebar + Content).
-   `DashboardForm.vb`: Halaman utama dengan statistik.
-   `MasterBarangForm.vb`: CRUD Barang table `tbl_barang`.
-   `KategoriForm.vb`: CRUD Kategori table `tbl_kategori`.
-   `BarangMasukForm.vb` & `BarangKeluarForm.vb`: Transaksi stok. Logic update stok ada di sini (event handlers).
-   `LaporanForm.vb`: Reporting module dengan query join table.

## Skema Database

### 1. `tbl_kategori`
-   `id_kategori` (INTEGER PK AutoIncrement)
-   `nama_kategori` (TEXT)

### 2. `tbl_barang`
-   `id_barang` (INTEGER PK AutoIncrement)
-   `kode_barang` (TEXT Unique)
-   `nama_barang` (TEXT)
-   `id_kategori` (INTEGER FK)
-   `satuan` (TEXT)
-   `harga_beli` (DECIMAL)
-   `harga_jual` (DECIMAL)
-   `stok` (INTEGER)
-   `stok_min` (INTEGER)

### 3. `tbl_barang_masuk`
-   `id_masuk` (PK)
-   `no_transaksi` (TEXT)
-   `tanggal` (TEXT YYYY-MM-DD)
-   `id_barang` (FK)
-   `jumlah` (INTEGER)
-   `keterangan` (TEXT)

### 4. `tbl_barang_keluar`
-   Mirip dengan barang masuk, mencatat transaksi keluar.

## Cara Build & Run

### Prasyarat
-   .NET 8.0 SDK diinstall.

### Perintah Terminal (CLI)

1.  **Restore Dependencies**:
    ```powershell
    dotnet restore src/StokBarangApp.vbproj
    ```

2.  **Jalankan (Debug Mode)**:
    ```powershell
    dotnet run --project src/StokBarangApp.vbproj
    ```

3.  **Build Release (Siap Pakai)**:
    ```powershell
    dotnet publish src/StokBarangApp.vbproj -c Release -r win-x64 --self-contained
    ```
    Hasil build ada di `src/bin/Release/net8.0-windows/win-x64/publish/`.

## Troubleshooting Umum

-   **Error "Table not found"**: Hapus file `stokbarang.db`, restart aplikasi untuk generate ulang.
-   **UI Berantakan di High DPI**: Aplikasi sudah diset `Application.SetHighDpiMode(HighDpiMode.SystemAware)`, pastikan scaling Windows optimal.
