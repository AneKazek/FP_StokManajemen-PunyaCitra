# 🔄 Transaksi Stok: Masuk & Keluar

Aplikasi ini menggunakan sistem *Double-Entry* sederhana untuk mencatat pergerakan stok, yaitu **Barang Masuk** dan **Barang Keluar**.

---

## 📥 Barang Masuk (Stock In)

Menu ini digunakan saat Anda menerima barang dari supplier atau hasil produksi sendiri.

### Langkah-langkah:
1.  Masuk ke menu **Barang Masuk**.
2.  **No. Transaksi**: Otomatis dibuat (Format: `IN-YYYYMMDD-XXXX`). Tidak perlu diubah.
3.  **Tanggal**: Default hari ini. Bisa diubah jika input data mundur (backdate).
4.  **Pilih Barang**:
    -   Klik tombol **[...]** atau **[Cari]** untuk membuka popup daftar barang.
    -   Pilih barang yang masuk, lalu klik [Pilih].
5.  **Jumlah Masuk**: Masukkan qty fisik yang diterima.
6.  **Keterangan**: (Opsional) Isi nama supplier, No. Surat Jalan, atau catatan lain.
7.  Klik **[Simpan]**.

### Efek Sistem:
-   Stok Barang di Master Data **bertambah (+)**.
-   Riwayat transaksi tercatat di tabel bawah.
-   Harga Beli Terakhir di Master Data akan diupdate (jika ada fitur average cost/last price update di masa depan, saat ini update stok saja).

---

## 📤 Barang Keluar (Stock Out)

Menu ini digunakan saat barang terjual, dipakai sendiri, rusak, atau dipindahkan.

### Langkah-langkah:
1.  Masuk ke menu **Barang Keluar**.
2.  **No. Transaksi**: Otomatis dibuat (Format: `OUT-YYYYMMDD-XXXX`).
3.  **Tanggal**: Default hari ini.
4.  **Pilih Barang**: Sama seperti Barang Masuk.
5.  **Jumlah Keluar**: Masukkan qty yang keluar.
6.  **Validasi Stok**:
    -   Sistem akan mengecek otomatis. Jika `Jumlah Keluar > Stok Saat Ini`, transaksi akan **DITOLAK**.
    -   Akan muncul pesan error "Stok tidak mencukupi!".
7.  **Keterangan**: (Opsional) Isi tujuan, nama pembeli, atau alasan keluar (misal: "Rusak/Expired").
8.  Klik **[Simpan]**.

### Efek Sistem:
-   Stok Barang di Master Data **berkurang (-)**.
-   Riwayat transaksi tercatat.

---

## 📋 Manajemen Riwayat Transaksi

Di bagian bawah setiap formulir transaksi terdapat tabel **Riwayat Harian**.

### Fitur Tabel Riwayat:
1.  **Lihat Data**: Menampilkan 150 transaksi terakhir.
2.  **Hapus Transaksi (Rollback)**:
    -   Jika Anda salah input, klik baris transaksi di tabel.
    -   Klik tombol **[Hapus]**.
    -   **PENTING**: Menghapus transaksi akan **mengembalikan stok** ke kondisi semula.
        -   Hapus Barang Masuk -> Stok berkurang.
        -   Hapus Barang Keluar -> Stok bertambah kembali.

> [!CAUTION]
> Hati-hati saat menghapus transaksi lama. Hal ini akan mengubah posisi stok saat ini dan bisa membuat laporan stok opname menjadi tidak akurat jika fisik barang sudah berubah.
