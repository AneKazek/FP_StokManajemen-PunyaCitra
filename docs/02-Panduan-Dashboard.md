# 📊 Panduan Dashboard

Dashboard adalah halaman pertama yang Anda lihat saat membuka aplikasi. Halaman ini dirancang untuk memberikan **"Helicopter View"** atau gambaran umum kondisi inventaris Anda dalam sekilas pandang.

## Komponen Dashboard

### 1. Kartu Ringkasan (Summary Cards)

Di bagian atas terdapat 4 kartu berwarna yang menunjukkan performa hari ini:

-   **🟦 Total Jenis Barang**:
    -   Menunjukkan jumlah *item unik* yang terdaftar di database.
    -   *Contoh*: Jika Anda punya 5 "Baut A" dan 3 "Baut B", ini akan terhitung 2 jenis barang.

-   **🟥 Stok Rendah**:
    -   **PENTING!** Angka ini menunjukkan jumlah barang yang stoknya berada di bawah batas minimum (`StokSaatIni <= StokMinimum`).
    -   Jika angka ini > 0, segera cek tabel "Peringatan Stok Rendah" di bawahnya.

-   **🟩 Masuk Hari Ini**:
    -   Jumlah total *transaksi* barang masuk yang dilakukan sejak jam 00:00 hari ini.
    -   Memberikan gambaran aktivitas penerimaan barang harian.

-   **🟨 Keluar Hari Ini**:
    -   Jumlah total *transaksi* barang keluar sejak jam 00:00 hari ini.
    -   Memberikan gambaran aktivitas pengeluaran/penjualan barang harian.

### 2. Tabel Peringatan Stok Rendah (Low Stock Alert)

Terletak di sisi kiri bawah dashboard. Tabel ini **otomatis menyaring** barang-barang kritis.

-   **Fungsi**: Memberitahu Anda barang apa saja yang perlu segera dipesan ulang (re-order).
-   **Kriteria Muncul**: Barang akan muncul di sini jika `Stok Saat Ini` ≤ `Stok Minimum` yang Anda set di Master Barang.
-   **Tindakan**: Jika tabel ini terisi, segera lakukan pembelian/produksi untuk barang tersebut.

### 3. Transaksi Terbaru (Recent Activity)

Terletak di sisi kanan bawah dashboard.

-   **Fungsi**: Menampilkan 10 riwayat transaksi terakhir secara real-time.
-   **Jenis Transaksi**: Mencampur data Barang Masuk (BM) dan Barang Keluar (BK) agar supervisor bisa memantau aktivitas gudang yang baru saja terjadi.
-   **Kolom**: Waktu, Jenis Transaksi, Nama Barang, Jumlah.

## Fitur Otomatis

-   **Auto-Refresh**: Dashboard akan memperbarui datanya secara otomatis setiap **30 detik**. Anda tidak perlu menekan tombol refresh manual untuk melihat perubahan stok terkini jika ada admin lain yang menginput data di jaringan yang sama (jika dikembangkan ke network) atau update otomatis dari transaksi baru.
-   **Jam Digital**: Menampilkan waktu server/komputer saat ini untuk validasi waktu transaksi.

## Tips Penggunaan

> [!TIP]
> **Biasakan Setiap Pagi**: Cek kartu **Stok Rendah**. Jika ada isinya, prioritaskan pengadaan barang tersebut sebelum memulai operasional harian agar tidak terjadi kehabisan stok (stockout).
