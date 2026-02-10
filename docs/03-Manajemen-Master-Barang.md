# 📦 Manajemen Master Data Barang

Menu **Master Barang** adalah pusat pengelolaan database produk Anda. Di sini Anda mendaftarkan item baru, mengubah harga, atau mengupdate info produk.

## Cara Mengakses

1.  Klik menu **"Master Barang"** di sidebar sebelah kiri.
2.  Halaman akan menampilkan daftar seluruh barang yang terdaftar.

## Fitur Utama

### 1. ➕ Tambah Barang Baru

Untuk mendaftarkan produk baru:
1.  Isi formulir di sebelah kiri (Input Form).
2.  **Kode Barang**: Otomatis di-generate saat pertama kali load, tapi bisa diubah manual.
    - *Tips*: Gunakan barcode scanner jika ada, klik field Kode Barang lalu scan.
3.  **Nama Barang**: Masukkan nama produk yang jelas.
4.  **Kategori**: Pilih dari dropdown. Jika belum ada, buat dulu di menu **Kategori**.
5.  **Satuan**: Contoh: Pcs, Box, Pack, Kg.
6.  **Harga Beli**: Harga modal awal per satuan.
7.  **Harga Jual**: Harga jual ke konsumen per satuan.
8.  **Stok Awal**: Stok yang ada saat ini di gudang.
9.  **Min. Stok**: Batas minimum untuk peringatan (Low Stock Alert).
10. Klik tombol **[Simpan]**.

### 2. ✏️ Edit Data Barang

Jika ada perubahan data (selain stok yang bergerak karena transaksi):
1.  Klik baris barang yang ingin diedit pada tabel di sebelah kanan.
2.  Data akan otomatis terisi ke formulir di sebelah kiri.
3.  Ubah data yang diperlukan (misal: Harga Jual naik).
4.  Klik tombol **[Update]**.

### 3. 🗑️ Hapus Barang

Untuk menghapus produk yang sudah tidak dijual:
1.  Pilih barang di tabel.
2.  Klik tombol **[Hapus]**.
3.  Konfirmasi dialog penghapusan.

> [!WARNING]
> Menghapus Master Barang tidak bisa dibatalkan jika sudah ada riwayat transaksi. Sebaiknya non-aktifkan (jika fitur tersedia) atau biarkan saja jika pernah bertransaksi demi integritas data laporan. Aplikasi ini saat ini mengizinkan penghapusan namun akan ada peringatan.

### 4. 🔍 Pencarian Barang

Gunakan kolom pencarian di atas tabel untuk menemukan barang dengan cepat:
-   Ketik nama barang, kode barang, atau kategori.
-   Tabel akan otomatis memfilter hasil sesuai kata kunci.

### 5. 🧹 Reset Form

Tombol **[Batal/Reset]** akan mengosongkan formulir input agar siap untuk entri data baru, tanpa mengubah data yang sedang dipilih di tabel.

## Tips Manajemen Data

-   **Kode Barang Unik**: Pastikan setiap varian produk memiliki Kode Barang yang berbeda.
-   **Kategori yang Rapi**: Kelompokkan barang dengan kategori yang logis (misal: Makanan, Minuman, Rokok) untuk memudahkan filter laporan nantinya.
-   **Stok Minimum**: Set nilai ini secara bijak. Jangan terlalu tinggi (boros gudang) atau terlalu rendah (risiko kehabisan).
