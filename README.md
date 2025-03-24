# Tubes 1 TankubanPrau
IF2211 - Strategi Algoritma
A Robocode Tank Battlle Royale Remake with own-designed tank bots that has each characteristics. A craft by team Tankuban Prau of Computer Science Undergraduate ITB 2023



i. Deskripsi Singkat
        1. BotMeriang (Greedy by Survival).
            - Bertahan hidup sebagai prioritas utama dengan menghindari peluru dan tetap bergerak.
            - Posisi strategis di pusat arena untuk meningkatkan peluang bertahan.
            - Gerakan melingkar untuk menghindari tembakan dan tetap aktif di arena.
            - Kecepatan maksimal untuk menghindari peluru musuh.
            - Menabrak musuh yang menempel di dinding untuk mengacaukan strategi lawan.
            - Menembak dengan firepower besar jika musuh dekat, firepower kecil jika jauh untuk efisiensi energi.
            - Jika energi di bawah 20, bot fokus bertahan dan menghindar.
2. Bot Ormas (Greedy by Target)
- Menyerang sebanyak mungkin dengan strategi tailing (mengekor target).
- Mendeteksi musuh terdekat (>100 pixel) dan mengikutinya.
- Menembak secara acak di awal untuk memperoleh skor cepat.
- Jika target mati atau menabrak tembok, bot akan mencari target baru.
- Jika terkena tembakan, bot akan berbelok ke kiri dan kanan bergantian untuk menghindari tembakan lanjutan.
- Jika energi di bawah 20, bot menempel dinding dan menembak sembarang untuk mendapatkan skor tambahan dan bertahan lebih lama.
3. Bot ChillPips (Greedy by Survival)
- Bertahan hidup selama mungkin untuk mendapatkan survival score.
- Bergerak ke tembok terdekat dan mengikuti jalur sepanjang tembok dengan kecepatan acak (5-8) untuk menghindari pola serangan.
- Menembak musuh dengan firepower kecil (1) jika jauh, dan firepower besar (3) jika dekat.
- Jika terjadi tabrakan, bot langsung berbalik arah untuk menghindari musuh dan mempertahankan strategi.
- Memanfaatkan kematian bot lain untuk meningkatkan survival score.
4. Bot KPR (Greedy by Energy)
- Menyerang bot dengan energi terkecil terlebih dahulu untuk mengurangi jumlah musuh dengan cepat.
- Menggunakan metode clustering untuk menentukan target prioritas berdasarkan energi terkecil.
- Mendekati cluster target dan menembak secara acak untuk memaksimalkan kemungkinan kill.
- Melakukan scan ulang jika ada bot yang mati atau jika terjadi tabrakan.
- Jika terkena tembakan atau ramming, bot akan berputar 90 derajat dan bergerak untuk menghindari serangan lanjutan.


ii. Requirement program dan instalasi
    Sebelum menjalankan Bot, pastikan Anda telah menginstal beberapa perangkat lunak berikut:
    **2.1.** Requirement Software
          - .NET SDK 6.0 atau lebih baru (https://dotnet.microsoft.com/download)
          - Robocode Tank Royale (https://robocode.io/)
          - Kode Bot
    **2.2.** Instalasi
          - Install .NET SDK:
          - Unduh dan instal .NET SDK dari situs resmi Microsoft
          - Pastikan instalasi berhasil dengan menjalankan: " dotnet --version "
          - Unduh Robocode Tank Royale:
          - Buka Robocode.io
          - Ikuti panduan instalasi sesuai sistem operasi Anda.
          - Clone atau Download Repository ini:

iii. Command atau langkah-langkah dalam meng-compile atau build program
        1. Clone repository ini
        2. Buka file “ robocode-tankroyale-gui-0.30.0.jar “ atau jalankan dengan terminal “java -jar robocode-tankroyale-gui-0.30.0.jar”
        3. Tambahkan folder boot root directories pada menu config di Robocode Tank Royale. 
        4. Klik start battle pada menu battle. 
        5. Pilih bot yang ingin digunakan. Lalu boot bot yang akan digunakan . 
        6. Add bot yang akan dijalankan pada permainan.
        7. Jalankan permainan dengan start battle.


iv. Author (identitas pembuat)
    - Stefan Mattew Susanto           13523020
    - Brian Albar Hadian              13523048
    - Angelina Efrina Prahastaputri   13523060
