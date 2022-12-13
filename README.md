
# AGIT x POLMAN dotnet security

Belajar coding security di .NET memakai JWT.


## Tentang JWT
JWT (Json Web Token)
JSON Web Token, yang berarti token ini menggunakan JSON (Javascript Object Notation) berbentuk string panjang yang sangat random, lalu token ini memungkinkan kita untuk mengirimkan data yang dapat diverifikasi oleh dua pihak atau lebih.

## Komponen penting
-secret\
Teks rahasia yang digunakan untuk membuat token atau identifikasi token.\
Contoh aplikasi ini memakai secret minimal 126 karakter.\
Contoh:
```http
  ihfbyldvwqiddnnwwmnnvngfdmaqgqlpedkcjqdghyvqwfiejkzhnlsqtkogmovzvczulhhmctoxeqprhanuqtrhdnrdydhlimboyipaagykyjhxjhcxixnhumsgkkkf
```
-token\
Token itu adalah kunci untuk membuka API private.\
Contoh:
```http
  eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImJ1ZGkiLCJuYmYiOjE2NzA5MTQzNjcsImV4cCI6MTY3MTAwMDc2NywiaWF0IjoxNjcwOTE0MzY3fQ.IE5QfAJdY3weOGuum1obBAb8r0oE2Jo9wOchqSPzYKs
```
untuk debug token bisa buka web ini
```http
  jwt.io
```

## Alur
1. user login dapat token
2. token digunakan di header Authorization