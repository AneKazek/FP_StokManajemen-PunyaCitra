# 📖 Overview Sistem Manajemen Stok Barang

## Pengenalan

**Sistem Manajemen Stok Barang** adalah aplikasi desktop modern yang dirancang untuk membantu bisnis, gudang, atau toko dalam mengelola inventaris mereka secara efisien. Aplikasi ini memberikan visibilitas real-time terhadap stok barang, memudahkan pencatatan barang masuk dan keluar, serta menyediakan laporan yang akurat.

## Fitur Utama

### 1. 📊 Dashboard Real-time
Pusat informasi utama yang menampilkan metrik penting secara instan:
- Total jenis barang yang terdaftar.
- Jumlah barang dengan stok menipis (perlu restock).
- Statistik transaksi barang masuk & keluar hari ini.
- Grafik atau tabel aktivitas terbaru.

### 2. 📦 Manajemen Master Data
Pengelolaan data inti inventaris:
- **Barang**: Mencatat kode, nama, kategori, satuan, harga beli/jual, dan lokasi penyimpanan.
- **Kategori**: Mengelompokkan barang agar mudah dicari dan dikelola.
- **Supplier & Tujuan**: (Opsional) Mencatat asal dan tujuan barang.

### 3. 🔄 Transaksi Stok (In/Out)
Pencatatan pergerakan barang yang akurat:
- **Barang Masuk (Stok In)**: Menambah stok saat pembelian atau penerimaan barang dari supplier. Otomatis update stok dan harga beli terakhir.
- **Barang Keluar (Stok Out)**: Mengurangi stok saat penjualan atau pemakaian internal. Sistem akan mencegah transaksi jika stok tidak mencukupi.

### 4. 📈 Laporan & Analisa
Pembuatan laporan otomatis untuk pengambilan keputusan:
- **Laporan Stok Saat Ini**: Mengetahui posisi stok terakhir beserta nilai aset.
- **Laporan Masuk/Keluar**: Rekapitulasi transaksi dalam periode tertentu.
- **Kartu Stok**: History pergerakan detail per item barang.
- **Export ke CSV**: Kemudahan untuk olah data lebih lanjut di Excel.

## Alur Kerja Sistem (Workflow)

1.  **Setup Awal**:
    - Input **Kategori Barang** (contoh: Elektronik, Bahan Baku, Packaging).
    - Input **Master Barang** (kode, nama, stok awal, harga).

2.  **Operasional Harian**:
    - Saat barang datang -> Input di menu **Barang Masuk**. Stok bertambah otomatis.
    - Saat barang dipakai/dijual -> Input di menu **Barang Keluar**. Stok berkurang otomatis.
    - Monitor **Dashboard** untuk melihat barang yang stoknya menipis.

3.  **Akhir Periode (Bulanan/Tahunan)**:
    - Buka menu **Laporan**.
    - Pilih periode tanggal.
    - Cetak atau Export laporan untuk audit fisik (Stock Opname).

## Keunggulan Teknis

- **Database Lokal (SQLite)**: Ringan, cepat, dan data tersimpan aman di komputer Anda (tidak butuh koneksi internet konstan).
- **UI Modern**: Tampilan bersih, nyaman di mata (Dark Mode Sidebar), dan mudah digunakan (User Friendly).
- **Portable**: Aplikasi bisa dipindahkan antar komputer dengan mudah cukup dengan menyalin folder aplikasi.
