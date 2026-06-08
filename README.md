# 🎮 TUTORIAL GAME EDUKASI 3D FREE ROAM SEKOLAH — UNITY
### Versi Revisi: First-Person, Jalan di Sekolah, Interact Objek, Win/Lose

---

## 🗺️ ALUR GAME (dari flowchart)

```
START
  │
  ▼
Main Menu ──► Pilih Level ──► Masuk Sekolah 3D (Free Roam FPS)
                                │
                                ▼
                        Jalan di Sekolah
                        Dekati Objek (Papan Tulis, Buku, Komputer, dll.)
                                │
                    ┌───────────┘
                    │  Tekan [E] → Baca Materi (teks)
                    │                    │
                    │                    ▼
                    │            Klik "MULAI TEST"
                    │                    │
                    │                    ▼
                    │            Kerjakan Soal Quiz
                    │                    │
                    │         ┌──────────┴──────────┐
                    │         ▼                     ▼
                    │    Nilai ≥ 70            Nilai < 70
                    │    [LULUS ✅]          [TIDAK LULUS ❌]
                    │    Objek → Hijau       Masih bisa coba lagi
                    │         │               (max 3x percobaan)
                    │         │               Jika habis → Objek Merah
                    └─────────┘
                                │
              ┌─────────────────┴──────────────────┐
              ▼                                     ▼
    Semua Objek Hijau ✅                  Gagal ≥ 3 Objek
    ┌────────────────┐                  ┌──────────────────┐
    │  🏆 WIN SCREEN │                  │  💀 LOSE SCREEN   │
    │  Skor + Unlock │                  │  Coba Lagi/Menu  │
    └────────────────┘                  └──────────────────┘
```

---

## 📁 FASE 1 — SETUP PROJECT 3D

### Langkah 1.1 — Buat Project Baru
1. Buka **Unity Hub → New Project**
2. Pilih template: **3D (Core)**
3. Nama: `GameEdukasi3D`
4. Klik **Create Project**

### Langkah 1.2 — Buat Struktur Folder
Klik kanan di panel **Project → Create → Folder**:
```
Assets/
├── Scripts/
├── Scenes/
├── Materials/
└── Prefabs/
```

### Langkah 1.3 — Buat 3 Scene
**File → New Scene → Save** sebanyak 3x:

| Nama Scene | Simpan di |
|------------|-----------|
| `MainMenu` | Assets/Scenes/ |
| `LevelSelect` | Assets/Scenes/ |
| `Sekolah` | Assets/Scenes/ ← level utama free roam |

### Langkah 1.4 — Daftarkan ke Build Settings
1. **File → Build Settings**
2. Drag semua scene ke kotak **Scenes In Build**
3. Urutan wajib:

| Index | Scene |
|-------|-------|
| 0 | MainMenu |
| 1 | LevelSelect |
| 2 | Sekolah |

### Langkah 1.5 — Buat Layer "Interactable" ⚠️ WAJIB
Layer ini dipakai oleh Raycast agar bisa mendeteksi objek interaktif.

1. **Edit → Project Settings → Tags and Layers**
2. Cari bagian **Layers**, cari slot kosong (misal User Layer 6)
3. Ketik: `Interactable`
4. Tutup Project Settings

---

## 🏫 FASE 2 — BANGUN SEKOLAH 3D (Pakai Primitive)

Buka scene **Sekolah**. Kita bangun ruangan kelas sederhana dulu.

### Langkah 2.1 — Buat Lantai
1. Hierarchy → klik kanan → **3D Object → Plane**
2. Rename: `Lantai`
3. **Transform:**
   - Position: (0, 0, 0)
   - Scale: (2, 1, 2) → luas 20×20 meter

### Langkah 2.2 — Buat 4 Dinding
Buat 4 **Cube** (klik kanan Hierarchy → 3D Object → Cube):

| Nama | Position | Scale |
|------|----------|-------|
| `DindingKiri` | (-10, 2, 0) | (0.3, 4, 20) |
| `DindingKanan` | (10, 2, 0) | (0.3, 4, 20) |
| `DindingDepan` | (0, 2, -10) | (20, 4, 0.3) |
| `DindingBelakang` | (0, 2, 10) | (20, 4, 0.3) |

### Langkah 2.3 — Buat Atap
1. Cube baru → rename `Atap`
2. Position: (0, 4, 0) — Scale: (20, 0.3, 20)

### Langkah 2.4 — Buat Objek Interaktif di Kelas
Buat Cube-cube ini sebagai objek pelajaran. Berikan warna berbeda di Material:

| Nama | Position | Scale | Fungsi |
|------|----------|-------|--------|
| `PapanTulis` | (0, 2, -9.5) | (6, 3, 0.2) | Materi Matematika |
| `MejaBuku` | (-5, 0.75, 3) | (2, 0.5, 1.5) | Materi IPA |
| `Komputer` | (5, 1.5, 3) | (0.8, 0.8, 0.5) | Materi Bahasa |
| `GlobeObjek` | (3, 1.2, -3) | (0.8, 0.8, 0.8) | Materi IPS |

> 💡 **Cara beri warna:** Project → Create → Material → atur warna di Inspector → drag ke objek

### Langkah 2.5 — Set Layer Interactable ke Semua Objek
Untuk **setiap objek interaktif** (PapanTulis, MejaBuku, Komputer, GlobeObjek):
1. Klik objek di Hierarchy
2. Di Inspector → bagian **Layer** (pojok kanan atas)
3. Pilih **Interactable**
4. Konfirmasi: pilih **"Yes, change children"**

### Langkah 2.6 — Rapikan Hierarchy
Buat Empty GameObject bernama `Sekolah_Environment`, lalu jadikan parent semua dinding dan lantai. Buat Empty lain bernama `ObjekInteraktif`, jadikan parent semua objek pelajaran.

---

## 🏃 FASE 3 — PLAYER CONTROLLER (FPS)

### Langkah 3.1 — Buat Struktur Player
Di Hierarchy, buat struktur ini:

```
Player (GameObject kosong)
└── PlayerCamera (Camera)
```

**Cara:**
1. Hierarchy → klik kanan → **Create Empty** → rename `Player`
2. Position: (0, 1.8, 5) ← spawn di tengah kelas
3. Klik kanan **Player** → **Camera** → rename `PlayerCamera`
4. PlayerCamera Position lokal: (0, 0, 0) ← tepat di kepala

### Langkah 3.2 — Tambah CharacterController ke Player
1. Klik **Player** di Hierarchy
2. Inspector → **Add Component** → ketik `Character Controller` → enter
3. Atur:
   - **Height**: 1.8
   - **Radius**: 0.35
   - **Center Y**: 0 (sudah oke default)

### Langkah 3.3 — Buat Script PlayerController

Di folder **Scripts**, klik kanan → Create → C# Script → nama: `PlayerController`

```csharp
using UnityEngine;

// Script ini WAJIB ada CharacterController di GameObject yang sama
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("=== KECEPATAN ===")]
    public float kecepatanJalan = 4f;
    public float kecepatanLari  = 7f;

    [Header("=== MOUSE LOOK ===")]
    public float sensitivitas = 2f;

    [Header("=== FISIKA ===")]
    public float gravitasi = -9.81f;

    // Referensi komponen
    private CharacterController cc;
    private Transform           camTransform;

    // State internal
    private float   rotasiX       = 0f;
    private Vector3 velocity;

    // Bisa diakses script lain untuk freeze/unfreeze player
    [HideInInspector] public bool bisaBergerak = true;

    void Start()
    {
        cc           = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;

        // Kunci cursor ke tengah layar saat game mulai
        SetKunciCursor(true);
    }

    void Update()
    {
        // Jika player di-freeze (saat buka panel UI), skip semua input
        if (!bisaBergerak) return;

        GerakWASD();
        LihatMouse();
    }

    // ─── Gerakan WASD + Shift lari ───────────────────────────────
    void GerakWASD()
    {
        // Atur gravitasi agar tidak "melayang" saat di tanah
        if (cc.isGrounded && velocity.y < 0f) velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical   = Input.GetAxis("Vertical");   // W / S
        bool  sedangLari = Input.GetKey(KeyCode.LeftShift);
        float kecepatan  = sedangLari ? kecepatanLari : kecepatanJalan;

        Vector3 gerak = transform.right * horizontal + transform.forward * vertical;
        cc.Move(gerak * kecepatan * Time.deltaTime);

        // Tambah gravitasi ke bawah
        velocity.y += gravitasi * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    // ─── Rotasi kamera dengan mouse ──────────────────────────────
    void LihatMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivitas;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivitas;

        // Rotasi vertikal kamera (atas-bawah), clamp agar tidak balik
        rotasiX -= mouseY;
        rotasiX  = Mathf.Clamp(rotasiX, -80f, 80f);
        camTransform.localRotation = Quaternion.Euler(rotasiX, 0f, 0f);

        // Rotasi horizontal player (kiri-kanan)
        transform.Rotate(Vector3.up * mouseX);
    }

    // ─── Dipanggil script lain saat buka/tutup panel UI ─────────
    public void SetBisaBergerak(bool status)
    {
        bisaBergerak = status;
        SetKunciCursor(status);
    }

    void SetKunciCursor(bool terkunci)
    {
        Cursor.lockState = terkunci ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible   = !terkunci;
    }
}
```

### Langkah 3.4 — Pasang Script ke Player
1. Klik **Player** di Hierarchy
2. Drag script `PlayerController.cs` dari panel Project ke Inspector

> ✅ **Test:** Klik Play, kamu harus bisa gerak WASD + lihat dengan mouse. Tekan Esc untuk keluar dari cursor lock.

---

## 🎯 FASE 4 — SISTEM INTERAKSI RAYCAST

Raycast adalah "sinar tak kasat mata" yang ditembak dari tengah layar ke depan. Kalau kena objek berlayer Interactable → tampilkan prompt [E].

### Langkah 4.1 — Buat Script PlayerInteraction

Scripts → Create → C# Script → nama: `PlayerInteraction`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("=== RAYCAST ===")]
    public float     jangkauan         = 3.5f;
    public LayerMask layerInteractable;          // set ke "Interactable" di Inspector

    [Header("=== UI PROMPT ===")]
    public GameObject promptPanel;  // panel kecil "[E] Papan Tulis"
    public Text       txtPrompt;    // teks di dalam prompt

    // Referensi internal
    private Camera             cam;
    private InteractableObject objekTarget;     // objek yang saat ini ditunjuk player

    void Start()
    {
        cam = Camera.main;
        promptPanel.SetActive(false);
    }

    void Update()
    {
        DeteksiObjekDiDepan();

        // Tekan E → interaksi dengan objek yang ditunjuk
        if (objekTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            objekTarget.Interaksi();
        }
    }

    void DeteksiObjekDiDepan()
    {
        // Tembak ray dari tepat tengah layar ke depan
        Ray        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, jangkauan, layerInteractable))
        {
            InteractableObject obj = hit.collider.GetComponent<InteractableObject>();

            // Pastikan ada komponen InteractableObject dan belum selesai permanen
            if (obj != null && !obj.sudahSelesaiPermanen)
            {
                // Ganti highlight jika target berubah
                if (objekTarget != obj)
                {
                    objekTarget?.SetHighlight(false); // matikan highlight lama
                    objekTarget = obj;
                    objekTarget.SetHighlight(true);   // nyalakan highlight baru
                }

                // Tampilkan prompt
                promptPanel.SetActive(true);
                txtPrompt.text = "[E]  " + obj.namaObjek;
                return;
            }
        }

        // Tidak ada objek terdeteksi → bersihkan
        objekTarget?.SetHighlight(false);
        objekTarget = null;
        promptPanel.SetActive(false);
    }
}
```

### Langkah 4.2 — Pasang Script ke PlayerCamera
1. Klik **PlayerCamera** di Hierarchy (bukan Player induknya!)
2. Drag script `PlayerInteraction.cs` ke Inspector
3. Di Inspector:
   - **Jangkauan**: 3.5
   - **Layer Interactable**: klik dropdown → centang **Interactable**
   - **Prompt Panel** dan **Txt Prompt** → isi nanti setelah buat Canvas UI

---

## 📦 FASE 5 — DATA SOAL (File Terpisah)

Kelas `Soal` dipakai oleh beberapa script, jadi kita buat file sendiri.

### Langkah 5.1 — Buat Script DataSoal

Scripts → Create → C# Script → nama: `DataSoal`

```csharp
using UnityEngine;
using System.Collections.Generic;

// [System.Serializable] agar bisa diedit di Inspector Unity
[System.Serializable]
public class Soal
{
    [TextArea(2, 5)]
    public string pertanyaan;

    // 4 pilihan jawaban (A, B, C, D)
    public string pilihanA;
    public string pilihanB;
    public string pilihanC;
    public string pilihanD;

    // Index jawaban benar: 0=A, 1=B, 2=C, 3=D
    [Range(0, 3)]
    public int indexJawabanBenar;

    // Helper: kembalikan array pilihan agar mudah diakses dengan loop
    public string[] GetPilihan()
    {
        return new string[] { pilihanA, pilihanB, pilihanC, pilihanD };
    }
}
```

---

## 🧱 FASE 6 — INTERACTABLE OBJECT 3D

### Langkah 6.1 — Buat Script InteractableObject

Scripts → Create → C# Script → nama: `InteractableObject`

```csharp
using UnityEngine;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    [Header("=== IDENTITAS OBJEK ===")]
    public string namaObjek = "Papan Tulis";

    [Header("=== KONTEN MATERI ===")]
    public string judulMateri = "Pelajaran Matematika";

    [TextArea(5, 20)]
    public string isiMateri = "Tulis isi materi pelajaran di sini...\n\nBisa panjang, ada scroll-nya.";

    [Header("=== SOAL QUIZ OBJEK INI ===")]
    public List<Soal> daftarSoal = new List<Soal>();

    [Header("=== PERCOBAAN ===")]
    [Tooltip("Berapa kali player bisa coba quiz ini sebelum dianggap gagal permanen")]
    public int maxPercobaan = 3;

    [Header("=== WARNA FEEDBACK ===")]
    public Color warnaNormal    = Color.white;
    public Color warnaHighlight = Color.yellow;
    public Color warnaLulus     = new Color(0.2f, 0.9f, 0.2f);  // hijau
    public Color warnaGagal     = new Color(0.9f, 0.2f, 0.2f);  // merah

    // ─── Status objek ───────────────────────────────────────────
    [HideInInspector] public bool sudahSelesaiPermanen = false;
    [HideInInspector] public int  jumlahPercobaanDilakukan = 0;

    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        AturWarna(warnaNormal);
    }

    // ─── Dipanggil PlayerInteraction saat tekan E ───────────────
    public void Interaksi()
    {
        if (sudahSelesaiPermanen) return;

        // Buka panel materi
        MaterialDisplay.Instance.TampilkanMateri(this);
    }

    // ─── Highlight saat ditunjuk / tidak ditunjuk ────────────────
    public void SetHighlight(bool aktif)
    {
        if (sudahSelesaiPermanen) return;
        AturWarna(aktif ? warnaHighlight : warnaNormal);
    }

    // ─── Dipanggil QuizManager setelah quiz selesai ──────────────
    public void TerimaHasil(bool lulus)
    {
        jumlahPercobaanDilakukan++;

        if (lulus)
        {
            // ✅ LULUS → objek selesai permanen, warna hijau
            sudahSelesaiPermanen = true;
            AturWarna(warnaLulus);
            SchoolManager.Instance.LaporkanObjekSelesai(true);
        }
        else
        {
            int sisaPercobaan = maxPercobaan - jumlahPercobaanDilakukan;

            if (sisaPercobaan <= 0)
            {
                // ❌ Habis percobaan → gagal permanen, warna merah
                sudahSelesaiPermanen = true;
                AturWarna(warnaGagal);
                SchoolManager.Instance.LaporkanObjekSelesai(false);
            }
            else
            {
                // ⚠️ Masih bisa coba lagi
                AturWarna(warnaNormal);
                SchoolManager.Instance.LaporkanGagalSementara(sisaPercobaan);
            }
        }
    }

    // ─── Helper warna ────────────────────────────────────────────
    void AturWarna(Color warna)
    {
        if (objRenderer != null)
            objRenderer.material.color = warna;
    }
}
```

### Langkah 6.2 — Pasang ke Setiap Objek 3D di Scene
Untuk **PapanTulis, MejaBuku, Komputer, GlobeObjek** satu per satu:

1. Klik objek di Hierarchy
2. Inspector → **Add Component** → ketik `InteractableObject` → enter
3. Isi data di Inspector:

**Contoh untuk PapanTulis:**
```
Nama Objek    : Papan Tulis
Judul Materi  : Matematika — Persamaan Linear
Isi Materi    : Persamaan linear adalah persamaan yang variabelnya...
               Bentuk umum: ax + b = c
               Contoh: 2x + 3 = 7
               Cara selesaikan: 2x = 7 - 3 = 4, maka x = 2

Daftar Soal   : (klik + untuk tambah soal)
  Soal 1:
    Pertanyaan : Jika 2x + 3 = 7, berapakah nilai x?
    Pilihan A  : 1
    Pilihan B  : 2   ← jawaban benar (index 1)
    Pilihan C  : 3
    Pilihan D  : 4
    Index Jawaban Benar : 1

  Soal 2:
    Pertanyaan : Bentuk sederhana dari 3x - x adalah...
    Pilihan A  : 4x
    Pilihan B  : 3x
    Pilihan C  : 2x   ← jawaban benar (index 2)
    Pilihan D  : x
    Index Jawaban Benar : 2

  Soal 3:
    Pertanyaan : Nilai x dari x + 5 = 12 adalah...
    Pilihan A  : 5
    Pilihan B  : 6
    Pilihan C  : 7   ← jawaban benar (index 2)
    Pilihan D  : 8
    Index Jawaban Benar : 2

Max Percobaan : 3
```

4. Pastikan **Layer** objek → **Interactable**

---

## 📖 FASE 7 — MATERIAL DISPLAY (Tampilan Materi)

### Langkah 7.1 — Buat Canvas UI Dulu

1. Buka scene **Sekolah**
2. Hierarchy → klik kanan → **UI → Canvas**
3. Di Inspector Canvas:
   - **Render Mode**: Screen Space - Overlay
4. Di dalam Canvas, buat **struktur UI lengkap**:

```
Canvas
│
├── Crosshair                  ← Image titik tengah (kecil, 10×10)
│
├── PanelPrompt                ← prompt interaksi, posisi bawah tengah
│   └── TxtPrompt              ← Text: "[E] Papan Tulis"
│
├── HUDPanel                   ← pojok kiri atas
│   ├── TxtProgress            ← Text: "Selesai: 0/4"
│   └── TxtGagal               ← Text: "Gagal: 0/3"
│
├── PanelMateri                ← popup materi BESAR, SetActive FALSE
│   ├── ImgBG                  ← Image background (warna gelap semi-transparan)
│   ├── TxtJudulMateri         ← Text judul besar
│   ├── ScrollView             ← untuk teks materi yang panjang
│   │   └── TxtIsiMateri       ← Text isi materi (di dalam ScrollView)
│   └── BtnMulaiQuiz           ← Button "📝 MULAI TEST"
│
├── PanelQuiz                  ← panel soal, SetActive FALSE
│   ├── TxtNomor               ← "Soal 1 / 3"
│   ├── TxtPertanyaan          ← teks pertanyaan
│   ├── BtnPilA                ← Button pilihan A
│   ├── BtnPilB                ← Button pilihan B
│   ├── BtnPilC                ← Button pilihan C
│   ├── BtnPilD                ← Button pilihan D
│   └── PanelHasil             ← hasil kuis, SetActive FALSE
│       ├── TxtNilai           ← "Nilai: 80 / 100"
│       ├── TxtPesan           ← "✅ Lulus!" / "❌ Belum lulus"
│       └── BtnLanjut          ← Button "LANJUT JELAJAH"
│
├── PanelWin                   ← WIN screen, SetActive FALSE
│   ├── TxtWin                 ← "🏆 KAMU MENANG!"
│   ├── TxtSkorWin             ← total skor
│   ├── BtnMainLagi            ← Button "MAIN LAGI"
│   └── BtnMenuWin             ← Button "KEMBALI MENU"
│
└── PanelLose                  ← LOSE screen, SetActive FALSE
    ├── TxtLose                ← "💀 GAME OVER"
    ├── TxtPesanLose           ← alasan kalah
    ├── BtnCobaLagi            ← Button "COBA LAGI"
    └── BtnMenuLose            ← Button "KEMBALI MENU"
```

> 💡 **Tips cepat:** Untuk PanelMateri agar terlihat bagus, set Image BG dengan warna hitam alpha ~180, dan pastikan Text bisa scroll dengan menambahkan **Scroll View** (UI → Scroll View).

### Langkah 7.2 — Buat Crosshair
1. Di dalam Canvas → UI → Image → rename `Crosshair`
2. Width: 10, Height: 10
3. Anchor: tengah layar (anchor preset middle center)
4. Warna: putih

### Langkah 7.3 — Buat Script MaterialDisplay

Scripts → Create → C# Script → nama: `MaterialDisplay`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class MaterialDisplay : MonoBehaviour
{
    // Singleton: akses dari mana saja dengan MaterialDisplay.Instance
    public static MaterialDisplay Instance;

    [Header("=== REFERENSI UI ===")]
    public GameObject panelMateri;
    public Text       txtJudul;
    public Text       txtIsiMateri;   // jika pakai teks biasa
    // Jika pakai ScrollView, drag Text yang ada di dalam ScrollView

    private InteractableObject objekAktif;
    private PlayerController   playerCtrl;

    void Awake()
    {
        Instance = this;
        panelMateri.SetActive(false);
    }

    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerController>();
    }

    // ─── Dipanggil InteractableObject.Interaksi() ───────────────
    public void TampilkanMateri(InteractableObject obj)
    {
        objekAktif        = obj;
        txtJudul.text     = obj.judulMateri;
        txtIsiMateri.text = obj.isiMateri;

        panelMateri.SetActive(true);

        // Freeze player + tampilkan cursor
        playerCtrl?.SetBisaBergerak(false);
    }

    // ─── Tombol "MULAI TEST" ─────────────────────────────────────
    public void TombolMulaiQuiz()
    {
        panelMateri.SetActive(false);
        QuizManager.Instance.MulaiKuis(objekAktif);
        // Player masih freeze sampai quiz selesai
    }

    // ─── (Opsional) Tombol "TUTUP" tanpa test ───────────────────
    public void TombolTutup()
    {
        panelMateri.SetActive(false);
        playerCtrl?.SetBisaBergerak(true);
    }
}
```

### Langkah 7.4 — Pasang MaterialDisplay ke Scene
1. Hierarchy → **Create Empty** → rename `MaterialDisplay`
2. Drag script `MaterialDisplay.cs` ke object ini
3. Di Inspector, isi referensi:
   - **Panel Materi** → drag `PanelMateri` dari Canvas
   - **Txt Judul** → drag `TxtJudulMateri`
   - **Txt Isi Materi** → drag `TxtIsiMateri`

### Langkah 7.5 — Hubungkan Tombol PanelMateri
- **BtnMulaiQuiz** On Click(): drag `MaterialDisplay` → `MaterialDisplay → TombolMulaiQuiz`

---

## 📝 FASE 8 — QUIZ MANAGER

### Langkah 8.1 — Buat Script QuizManager

Scripts → Create → C# Script → nama: `QuizManager`

```csharp
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [Header("=== UI PANEL QUIZ ===")]
    public GameObject panelQuiz;
    public Text       txtNomor;          // "Soal 1 / 3"
    public Text       txtPertanyaan;
    public Button[]   tombolPilihan;     // 4 tombol: A, B, C, D — ukuran array = 4

    [Header("=== UI PANEL HASIL ===")]
    public GameObject panelHasil;
    public Text       txtNilai;          // "Nilai: 80 / 100"
    public Text       txtPesan;          // Lulus / tidak lulus + sisa percobaan
    public Button     btnLanjut;         // "LANJUT JELAJAH"

    [Header("=== PENGATURAN ===")]
    [Range(0, 100)]
    public int nilaiMinimumLulus = 70;

    // ─── Data internal kuis yang sedang berjalan ─────────────────
    private InteractableObject objekSaatIni;
    private Soal[]             soalArray;
    private int                indexSoal;
    private int                jumlahJawabanBenar;

    private PlayerController playerCtrl;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        panelQuiz.SetActive(false);
        panelHasil.SetActive(false);
        playerCtrl = FindObjectOfType<PlayerController>();
    }

    // ─── Dipanggil MaterialDisplay.TombolMulaiQuiz() ─────────────
    public void MulaiKuis(InteractableObject obj)
    {
        objekSaatIni       = obj;
        soalArray          = obj.daftarSoal.ToArray();
        indexSoal          = 0;
        jumlahJawabanBenar = 0;

        panelHasil.SetActive(false);
        panelQuiz.SetActive(true);
        TampilkanSoal();
    }

    void TampilkanSoal()
    {
        // Semua soal sudah dijawab → hitung hasil
        if (indexSoal >= soalArray.Length)
        {
            HitungHasil();
            return;
        }

        Soal s = soalArray[indexSoal];

        // Isi teks soal
        txtNomor.text      = "Soal  " + (indexSoal + 1) + "  /  " + soalArray.Length;
        txtPertanyaan.text = s.pertanyaan;

        // Isi teks dan listener setiap tombol pilihan
        string[] pilihan = s.GetPilihan();
        string[] labelHuruf = { "A", "B", "C", "D" };

        for (int i = 0; i < tombolPilihan.Length; i++)
        {
            // Teks tombol: "A.  Jawaban pilihan"
            tombolPilihan[i].GetComponentInChildren<Text>().text =
                labelHuruf[i] + ".  " + pilihan[i];

            tombolPilihan[i].interactable = true;

            int capI = i;  // ⚠️ WAJIB capture variable untuk lambda
            tombolPilihan[i].onClick.RemoveAllListeners();
            tombolPilihan[i].onClick.AddListener(() => PilihJawaban(capI));
        }
    }

    void PilihJawaban(int indexDipilih)
    {
        // Nonaktifkan semua tombol agar tidak bisa klik ganda
        foreach (Button btn in tombolPilihan)
            btn.interactable = false;

        // Cek apakah benar
        if (indexDipilih == soalArray[indexSoal].indexJawabanBenar)
            jumlahJawabanBenar++;

        indexSoal++;

        // Jeda kecil sebelum pindah soal berikutnya
        Invoke(nameof(TampilkanSoal), 0.4f);
    }

    void HitungHasil()
    {
        panelQuiz.SetActive(false);
        panelHasil.SetActive(true);

        // Hitung nilai 0–100
        float persentase = (float)jumlahJawabanBenar / soalArray.Length;
        int   nilaiAkhir = Mathf.RoundToInt(persentase * 100f);
        bool  lulus      = nilaiAkhir >= nilaiMinimumLulus;

        // Tampilkan nilai
        txtNilai.text = "Nilai Kamu:  " + nilaiAkhir + " / 100\n"
                      + "( " + jumlahJawabanBenar + " benar dari " + soalArray.Length + " soal )";

        if (lulus)
        {
            txtPesan.text = "✅  LULUS!\nSelamat, kamu berhasil!";
        }
        else
        {
            int sisaPercobaan = objekSaatIni.maxPercobaan
                              - objekSaatIni.jumlahPercobaanDilakukan - 1;

            if (sisaPercobaan > 0)
                txtPesan.text = "❌  Belum Lulus\nNilai minimum: " + nilaiMinimumLulus
                              + "\nSisa percobaan: " + sisaPercobaan + " kali lagi";
            else
                txtPesan.text = "❌  Gagal!\nTidak ada percobaan tersisa.";
        }

        // Kirim hasil ke objek interaktif
        objekSaatIni.TerimaHasil(lulus);

        // Simpan skor ke GameManager
        GameManager.Instance?.SimpanSkor(nilaiAkhir);
    }

    // ─── Tombol "LANJUT JELAJAH" ─────────────────────────────────
    public void TombolLanjut()
    {
        panelHasil.SetActive(false);

        // Kembalikan kontrol ke player untuk lanjut jalan
        playerCtrl?.SetBisaBergerak(true);
    }
}
```

### Langkah 8.2 — Setup QuizManager di Scene
1. Hierarchy → **Create Empty** → rename `QuizManager`
2. Drag script ke object
3. Di Inspector:
   - **Panel Quiz** → drag `PanelQuiz`
   - **Txt Nomor** → drag `TxtNomor`
   - **Txt Pertanyaan** → drag `TxtPertanyaan`
   - **Tombol Pilihan** → Size: **4** → drag BtnPilA, BtnPilB, BtnPilC, BtnPilD
   - **Panel Hasil** → drag `PanelHasil`
   - **Txt Nilai** → drag `TxtNilai`
   - **Txt Pesan** → drag `TxtPesan`
   - **Btn Lanjut** → drag `BtnLanjut`
   - **Nilai Minimum Lulus**: 70

### Langkah 8.3 — Hubungkan Tombol Quiz
- **BtnLanjut** On Click(): drag `QuizManager` → `QuizManager → TombolLanjut`

---

## 🏆 FASE 9 — SCHOOL MANAGER (WIN / LOSE CONDITION)

### Langkah 9.1 — Buat Script SchoolManager

Scripts → Create → C# Script → nama: `SchoolManager`

```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SchoolManager : MonoBehaviour
{
    public static SchoolManager Instance;

    [Header("=== SEMUA OBJEK INTERAKTIF ===")]
    [Tooltip("Drag semua InteractableObject dari scene ke sini")]
    public InteractableObject[] semuaObjek;

    [Header("=== KONDISI LOSE ===")]
    [Tooltip("Jika total objek yang gagal permanen >= angka ini → Game Over")]
    public int maxObjekGagal = 2;

    [Header("=== UI WIN ===")]
    public GameObject panelWin;
    public Text       txtSkorWin;

    [Header("=== UI LOSE ===")]
    public GameObject panelLose;
    public Text       txtPesanLose;

    [Header("=== HUD (pojok layar) ===")]
    public Text txtProgress;  // "Selesai: 2 / 4"
    public Text txtGagal;     // "Gagal: 1 / 2"

    // ─── Counter internal ─────────────────────────────────────────
    private int totalSelesai     = 0;
    private int totalLulus       = 0;
    private int totalGagalPermanen = 0;

    private PlayerController playerCtrl;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerCtrl = FindObjectOfType<PlayerController>();
        panelWin.SetActive(false);
        panelLose.SetActive(false);
        UpdateHUD();
    }

    // ─── Dipanggil InteractableObject.TerimaHasil() ───────────────
    // lulus = true  → objek berhasil diselesaikan (hijau)
    // lulus = false → objek gagal permanen (merah, kehabisan percobaan)
    public void LaporkanObjekSelesai(bool lulus)
    {
        totalSelesai++;

        if (lulus)
        {
            totalLulus++;
        }
        else
        {
            totalGagalPermanen++;

            // ─── Cek LOSE: terlalu banyak gagal permanen ──────────
            if (totalGagalPermanen >= maxObjekGagal)
            {
                TriggerLose("Kamu gagal di terlalu banyak pelajaran.\nBelajar lagi ya!");
                return;
            }
        }

        UpdateHUD();

        // ─── Cek apakah semua objek sudah selesai (lulus/gagal) ───
        if (totalSelesai >= semuaObjek.Length)
        {
            if (totalLulus >= semuaObjek.Length)
            {
                // Semua lulus → WIN!
                TriggerWin();
            }
            else
            {
                // Ada yang gagal, tapi belum trigger lose sebelumnya
                TriggerLose("Tidak semua pelajaran berhasil diselesaikan.");
            }
        }
    }

    // ─── Dipanggil saat gagal tapi masih ada sisa percobaan ──────
    public void LaporkanGagalSementara(int sisaPercobaan)
    {
        UpdateHUD();
        Debug.Log("Gagal! Masih ada " + sisaPercobaan + " percobaan lagi.");
    }

    // ─── WIN ──────────────────────────────────────────────────────
    void TriggerWin()
    {
        panelWin.SetActive(true);

        int skorTerbaik = 0;
        if (GameManager.Instance != null)
            skorTerbaik = GameManager.Instance.highScore[GameManager.Instance.currentLevel];

        txtSkorWin.text = "🏆 LUAR BIASA!\n"
                        + "Kamu menyelesaikan semua pelajaran!\n\n"
                        + "Skor Terbaik Level Ini: " + skorTerbaik;

        // Buka level berikutnya
        GameManager.Instance?.BukaLevelBerikutnya();

        // Freeze player
        playerCtrl?.SetBisaBergerak(false);
    }

    // ─── LOSE ─────────────────────────────────────────────────────
    void TriggerLose(string alasan)
    {
        panelLose.SetActive(true);
        txtPesanLose.text = "💀 GAME OVER\n\n" + alasan;

        // Freeze player
        playerCtrl?.SetBisaBergerak(false);
    }

    // ─── Update HUD pojok layar ───────────────────────────────────
    void UpdateHUD()
    {
        if (txtProgress != null)
            txtProgress.text = "Selesai:  " + totalSelesai + " / " + semuaObjek.Length;

        if (txtGagal != null)
            txtGagal.text    = "Gagal:  " + totalGagalPermanen + " / " + maxObjekGagal;
    }

    // ─── Tombol di panel Win/Lose ─────────────────────────────────
    public void TombolMainLagi()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TombolKembaliMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
```

### Langkah 9.2 — Setup SchoolManager di Scene
1. Hierarchy → **Create Empty** → rename `SchoolManager`
2. Drag script `SchoolManager.cs`
3. Di Inspector:
   - **Semua Objek** → Size: **4** (sesuai jumlah objek)
   - Drag PapanTulis, MejaBuku, Komputer, GlobeObjek ke slot
   - **Max Objek Gagal**: 2 ← jika 2 objek gagal → Lose
   - **Panel Win/Lose, HUD** → drag dari Canvas

### Langkah 9.3 — Hubungkan Tombol Win/Lose

| Tombol | On Click() |
|--------|-----------|
| BtnMainLagi (Win) | `SchoolManager → TombolMainLagi` |
| BtnMenuWin | `SchoolManager → TombolKembaliMenu` |
| BtnCobaLagi (Lose) | `SchoolManager → TombolMainLagi` |
| BtnMenuLose | `SchoolManager → TombolKembaliMenu` |

---

## 🧠 FASE 10 — GAME MANAGER (Simpan Progress)

### Langkah 10.1 — Buat Script GameManager

Scripts → Create → C# Script → nama: `GameManager`

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("=== PENGATURAN ===")]
    public int jumlahLevel = 3;

    [HideInInspector] public int    currentLevel  = 0;
    [HideInInspector] public int[]  highScore;
    [HideInInspector] public bool[] levelTerbuka;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // hidup di semua scene
            Inisialisasi();
        }
        else
        {
            Destroy(gameObject); // buang duplikat
        }
    }

    void Inisialisasi()
    {
        highScore    = new int[jumlahLevel];
        levelTerbuka = new bool[jumlahLevel];
        levelTerbuka[0] = true; // level 1 selalu terbuka

        for (int i = 0; i < jumlahLevel; i++)
        {
            highScore[i]    = PlayerPrefs.GetInt("Score_" + i, 0);
            levelTerbuka[i] = (i == 0) ||
                              (PlayerPrefs.GetInt("Level_" + i, 0) == 1);
        }
    }

    public void SimpanSkor(int skor)
    {
        if (skor > highScore[currentLevel])
        {
            highScore[currentLevel] = skor;
            PlayerPrefs.SetInt("Score_" + currentLevel, skor);
            PlayerPrefs.Save();
        }
    }

    public void BukaLevelBerikutnya()
    {
        int next = currentLevel + 1;
        if (next < jumlahLevel)
        {
            levelTerbuka[next] = true;
            PlayerPrefs.SetInt("Level_" + next, 1);
            PlayerPrefs.Save();
        }
    }

    public void ResetSemuaProgress()
    {
        PlayerPrefs.DeleteAll();
        Inisialisasi();
    }
}
```

### Langkah 10.2 — Pasang di Scene MainMenu
1. Buka scene **MainMenu**
2. Hierarchy → Create Empty → rename `GameManager`
3. Drag script `GameManager.cs`
4. **Jumlah Level**: 3

> ✅ Karena DontDestroyOnLoad, GameManager akan ikut ke scene manapun otomatis.

---

## 🏠 FASE 11 — MAIN MENU & LEVEL SELECT

### Langkah 11.1 — Script MainMenuUI

Scripts → `MainMenuUI.cs`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void TombolMain()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void TombolKeluar()
    {
        Application.Quit();
    }
}
```

**Setup scene MainMenu:**
- Canvas → Text "GAME EDUKASI 3D SEKOLAH"
- Button "MULAI" → On Click(): `MainMenuUI → TombolMain`
- Button "KELUAR" → On Click(): `MainMenuUI → TombolKeluar`

---

### Langkah 11.2 — Script LevelSelectUI

Scripts → `LevelSelectUI.cs`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [Tooltip("Drag semua tombol level ke sini")]
    public Button[] tombolLevel;

    void Start()
    {
        if (GameManager.Instance == null) return;

        for (int i = 0; i < tombolLevel.Length; i++)
        {
            bool bisa = GameManager.Instance.levelTerbuka[i];
            tombolLevel[i].interactable = bisa;

            // Abu-abu jika terkunci
            ColorBlock warna = tombolLevel[i].colors;
            warna.normalColor = bisa ? Color.white : Color.gray;
            tombolLevel[i].colors = warna;
        }
    }

    // Dipanggil On Click() tiap tombol level
    // Set angka argumen di Inspector: Level1=0, Level2=1, Level3=2
    public void PilihLevel(int index)
    {
        GameManager.Instance.currentLevel = index;
        // Level1 → scene "Sekolah", bisa duplikat jadi Sekolah2, Sekolah3
        SceneManager.LoadScene("Sekolah");
    }

    public void TombolKembali()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
```

**Setup scene LevelSelect:**
- Canvas → Text "PILIH LEVEL"
- Button "LEVEL 1" → On Click(): `LevelSelectUI → PilihLevel` → angka `0`
- Button "LEVEL 2" → On Click(): `LevelSelectUI → PilihLevel` → angka `1`
- Button "LEVEL 3" → On Click(): `LevelSelectUI → PilihLevel` → angka `2`
- Button "KEMBALI" → On Click(): `LevelSelectUI → TombolKembali`

---

## ⚙️ FASE 12 — SETUP AKHIR: HUBUNGKAN SEMUA

### Langkah 12.1 — Checklist Semua GameObject di Scene Sekolah

| GameObject | Script Yang Dipasang | Referensi Wajib Diisi |
|-----------|---------------------|-----------------------|
| `GameManager` (di MainMenu) | GameManager.cs | Jumlah Level: 3 |
| `Player` | PlayerController.cs | — |
| `PlayerCamera` (child) | PlayerInteraction.cs | Layer, PromptPanel, TxtPrompt |
| `MaterialDisplay` | MaterialDisplay.cs | PanelMateri, TxtJudul, TxtIsi |
| `QuizManager` | QuizManager.cs | PanelQuiz, 4 TombolPilihan, PanelHasil |
| `SchoolManager` | SchoolManager.cs | SemuaObjek[], PanelWin, PanelLose, HUD |
| `PapanTulis` | InteractableObject.cs | Nama, Materi, Soal, Layer=Interactable |
| `MejaBuku` | InteractableObject.cs | Nama, Materi, Soal, Layer=Interactable |
| `Komputer` | InteractableObject.cs | Nama, Materi, Soal, Layer=Interactable |
| `GlobeObjek` | InteractableObject.cs | Nama, Materi, Soal, Layer=Interactable |

### Langkah 12.2 — Semua On Click() Lengkap

**Scene MainMenu:**

| Tombol | On Click() |
|--------|-----------|
| BtnMulai | MainMenuUI → TombolMain |
| BtnKeluar | MainMenuUI → TombolKeluar |

**Scene LevelSelect:**

| Tombol | On Click() | Argumen |
|--------|-----------|---------|
| BtnLevel1 | LevelSelectUI → PilihLevel | 0 |
| BtnLevel2 | LevelSelectUI → PilihLevel | 1 |
| BtnLevel3 | LevelSelectUI → PilihLevel | 2 |
| BtnKembali | LevelSelectUI → TombolKembali | — |

**Scene Sekolah (in-game UI):**

| Tombol | On Click() |
|--------|-----------|
| BtnMulaiQuiz | MaterialDisplay → TombolMulaiQuiz |
| BtnLanjut | QuizManager → TombolLanjut |
| BtnMainLagi (Win) | SchoolManager → TombolMainLagi |
| BtnMenuWin | SchoolManager → TombolKembaliMenu |
| BtnCobaLagi (Lose) | SchoolManager → TombolMainLagi |
| BtnMenuLose | SchoolManager → TombolKembaliMenu |

### Langkah 12.3 — Setup PlayerInteraction (Penting!)
1. Klik **PlayerCamera** di Hierarchy
2. Di Inspector → komponen PlayerInteraction:
   - **Jangkauan**: 3.5
   - **Layer Interactable**: klik dropdown → **centang Interactable**
   - **Prompt Panel**: drag `PanelPrompt` dari Canvas
   - **Txt Prompt**: drag `TxtPrompt` dari dalam PanelPrompt

### Langkah 12.4 — Pastikan Collider & Layer Benar
Untuk setiap objek interaktif (PapanTulis, MejaBuku, dll.):
1. Klik objek di Hierarchy
2. Cek ada komponen **Box Collider** (Cube default sudah punya)
3. Cek **Layer** di pojok kanan atas Inspector → harus **Interactable**

---

## 🧪 FASE 13 — CARA TEST CORE LOOP

### Urutan Play:
1. Buka scene **MainMenu** → klik **▶ Play**
2. Klik **MULAI** → masuk LevelSelect
3. Klik **LEVEL 1** → masuk scene Sekolah

### Di Scene Sekolah:
```
1. Jalan dengan WASD, lihat dengan mouse
2. Dekati PapanTulis → muncul "[E] Papan Tulis"
3. Tekan E → Panel Materi muncul, cursor terbuka
4. Baca materi → klik "MULAI TEST"
5. Jawab soal A/B/C/D
6. Lihat hasil: nilai + lulus/tidak
7. Klik "LANJUT JELAJAH" → bisa gerak lagi
8. PapanTulis sekarang warna HIJAU (lulus) atau tetap putih (gagal, masih bisa coba)
9. Ulangi untuk semua objek lain
10. Jika semua HIJAU → WIN screen muncul 🏆
11. Jika gagal 2x permanen → LOSE screen muncul 💀
```

---

## 🐛 FASE 14 — TROUBLESHOOTING

### ❌ "NullReferenceException: Object reference not set..."
**Penyebab paling umum:** Play scene bukan dari MainMenu, jadi GameManager tidak ada.
**Solusi:** Selalu Play dari scene **MainMenu**, atau tambah null check:
```csharp
// Tambah di semua pemanggilan GameManager
GameManager.Instance?.SimpanSkor(nilai);  // tanda ? = skip jika null
```

---

### ❌ Raycast tidak mendeteksi objek sama sekali
Cek 3 hal ini berurutan:
1. Layer objek sudah **Interactable**? (Inspector → Layer dropdown)
2. LayerMask di PlayerInteraction sudah **dicentang Interactable**?
3. Objek punya **Collider**? (Cube default punya BoxCollider)
4. Jangkauan cukup? Coba naikkan ke 5

---

### ❌ Prompt [E] muncul tapi tekan E tidak terjadi apa-apa
**Penyebab:** Script `PlayerInteraction` tidak ada di **PlayerCamera**, tapi di Player.
**Solusi:** Pastikan `PlayerInteraction.cs` dipasang di **PlayerCamera** (child), bukan di Player (parent).

---

### ❌ Kursor tidak muncul saat panel terbuka, tidak bisa klik tombol
**Penyebab:** `SetBisaBergerak(false)` tidak dipanggil, cursor masih locked.
**Solusi:** Pastikan `MaterialDisplay.TampilkanMateri()` memanggil `playerCtrl?.SetBisaBergerak(false)`. Cek script MaterialDisplay.

---

### ❌ Player tembus dinding
**Penyebab:** Dinding tidak punya Collider, atau CharacterController collision tidak aktif.
**Solusi:**
1. Klik dinding → Inspector → cek ada **Box Collider** → jika tidak ada, Add Component → Box Collider
2. Atau: gunakan Static Collider (centang **Static** di pojok kanan atas Inspector setiap dinding)

---

### ❌ Kamera berputar terbalik (atas-bawah)
**Penyebab:** Sensitivitas mouse negatif, atau rotasiX terbalik.
**Solusi:** Di script PlayerController, ubah tanda: `rotasiX += mouseY;` (ganti minus jadi plus) atau sebaliknya.

---

### ❌ Soal tidak tampil / teks tombol kosong
**Penyebab:** Unity versi baru memakai **TextMeshPro** bukan legacy `Text`.
**Solusi:** Ganti baris ini di QuizManager:
```csharp
// LAMA (legacy Text):
tombolPilihan[i].GetComponentInChildren<Text>().text = ...

// BARU (TextMeshPro):
using TMPro;
tombolPilihan[i].GetComponentInChildren<TextMeshProUGUI>().text = ...
```
Pastikan di atas file ada `using TMPro;`

---

### ❌ Scene tidak pindah saat klik tombol
**Penyebab:** Nama scene di `SceneManager.LoadScene("NamaScene")` tidak cocok.
**Solusi:**
1. Cek **File → Build Settings** → pastikan semua scene terdaftar
2. Nama di LoadScene harus **persis sama** termasuk huruf kapital
3. Contoh: `"Sekolah"` ≠ `"sekolah"` ≠ `"SEKOLAH"`

---

## 📂 STRUKTUR FILE FINAL

```
Assets/
├── Scripts/
│   ├── GameManager.cs           ← Otak game, simpan progress
│   ├── PlayerController.cs      ← Gerak WASD + mouse FPS
│   ├── PlayerInteraction.cs     ← Raycast + tekan E
│   ├── DataSoal.cs              ← Class Soal (dipakai banyak script)
│   ├── InteractableObject.cs    ← Data + logika objek 3D
│   ├── MaterialDisplay.cs       ← Panel tampilan materi
│   ├── QuizManager.cs           ← Sistem soal + nilai
│   ├── SchoolManager.cs         ← Win/Lose condition
│   ├── MainMenuUI.cs            ← Tombol main menu
│   └── LevelSelectUI.cs         ← Pilih level + lock/unlock
│
├── Scenes/
│   ├── MainMenu.unity
│   ├── LevelSelect.unity
│   └── Sekolah.unity            ← Scene utama free roam
│
├── Materials/
│   ├── MatLantai.mat
│   ├── MatDinding.mat
│   ├── MatPapanTulis.mat
│   └── ... (warna tiap objek)
│
└── Prefabs/
    └── Player.prefab            ← Simpan Player sebagai prefab
```

---

## 🚀 TIPS PENGEMBANGAN SELANJUTNYA

### Tingkat Sederhana:
- **Lebih banyak ruangan:** Duplikat dinding, buat koridor, tambah kelas baru
- **Musik & SFX:** AudioSource di Player + AudioClip untuk footstep, interaksi, benar/salah
- **Timer per quiz:** Countdown timer di panel kuis agar lebih menantang

### Tingkat Menengah:
- **NPC Guru:** Buat karakter dengan NavMeshAgent, bisa diklik untuk dapat hint
- **Minimap:** Kamera kedua orthographic dari atas, Render ke RenderTexture di pojok layar
- **Animasi panel UI:** Gunakan Animator atau DOTween (free asset) untuk slide-in/slide-out
- **Highlight outline:** Import package **Quick Outline** dari Asset Store untuk efek glow objek

### Tingkat Lanjut:
- **Beberapa lantai sekolah:** NavMesh + tangga, atau gunakan teleport trigger
- **Leaderboard lokal:** Tampilkan 5 skor terbaik di LevelSelect
- **Video materi:** Tambah komponen `VideoPlayer` Unity untuk putar video lokal/URL

---

*Tutorial dibuat untuk Unity 2020 LTS ke atas.*
*Selamat berkarya! 🎓🎮*
