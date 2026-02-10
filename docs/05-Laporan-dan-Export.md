# 📈 Laporan & Export Data

Menu **Laporan** menyediakan analisis komprehensif mengenai pergerakan stok Anda. Laporan ini bisa dicetak atau diexport ke format Excel (CSV).

## Jenis Laporan

### 1. 📊 Laporan Stok Saat Ini (Current Stock)

-   **Fungsi**: Mengetahui posisi real stok per hari ini.
-   **Kolom**: Kode Barang, Nama Barang, Kategori, Stok, Satuan, Harga Beli.
-   **Total Aset**: Di bagian bawah laporan akan muncul total nilai aset barang yang ada di gudang (Total = Σ (Stok * Harga Beli)).

### 2. 📥 Laporan Barang Masuk (Received Goods)

-   **Fungsi**: Merekap semua transaksi penerimaan barang dalam periode tertentu.
-   **Filter Tanggal**: Pilih "Dari Tanggal" dan "Sampai Tanggal" untuk melihat data dalam rentang waktu spesifik (misal: Bulan Ini).
-   **Kegunaan**: Cocokkan dengan faktur pembelian dari supplier.

### 3. 📤 Laporan Barang Keluar (Issued Goods)

-   **Fungsi**: Merekap semua transaksi pengeluaran barang.
-   **Filter Tanggal**: Bisa difilter per periode.
-   **Kegunaan**: Analisis barang apa yang paling laku (fast moving) atau rekap pemakaian barang.

### 4. 📇 Kartu Stok (Stock Card)

-   **Fungsi**: Laporan mendetail pergerakan **satu jenis barang**.
-   **Tampilan**: Menunjukkan history Masuk, Keluar, dan Saldo Akhir secara kronologis.
-   **Cara Pakai**:
    1.  Pilih Tab "Kartu Stok".
    2.  Pilih Barang yang ingin dicek.
    3.  Tentukan periode tanggal.
    4.  Klik **[Tampilkan]**.

## Fitur Export (Simpan ke Excel)

Setiap laporan memiliki tombol **[Export CSV]** di pojok kanan atas tabel.

1.  Tampilkan laporan yang diinginkan.
2.  Klik tombol **[Export CSV]**.
3.  Akan muncul dialog penyimpanan file (Save As).
4.  Pilih lokasi penyimpanan di komputer Anda, beri nama file.
5.  File `.csv` yang dihasilkan bisa dibuka langsung dengan **Microsoft Excel**, **Google Sheets**, atau **Numbers**.

> [!NOTE]
> Format CSV (Comma Separated Values) adalah standar universal pertukaran data. Jika saat dibuka di Excel berantakan (menjadi satu kolom), gunakan fitur "Text to Columns" di Excel dengan pemisah koma (comma) atau titik koma (semicolon) tergantung setting region komputer Anda.
