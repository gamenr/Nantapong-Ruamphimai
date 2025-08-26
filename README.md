# Technical Decision Log - UX/UI Mobile Game

**Project:** Quest & Chest  
**Date:** 25-27 August 2025  
**Unity Version:** 6000.0.56f1 LTS  

---

## Quest & Chest 
<img width="450" height="871" alt="image" src="https://github.com/user-attachments/assets/61605368-c0d4-4bb8-9563-1978361ccc49" />
<img width="429" height="869" alt="image" src="https://github.com/user-attachments/assets/a5b69dc1-8208-450d-b852-7c76916b3783" />



## UI References  

### Fantasy Items Pack  
<img width="1024" height="396" alt="image" src="https://github.com/user-attachments/assets/6904115a-1963-4bef-b4dd-0b092132146d" />
  

### RPG & Fantasy Mobile GUI  
<img width="1024" height="406" alt="image" src="https://github.com/user-attachments/assets/1038562a-c7c9-4398-832e-e189734aaacb" />
  

---

## UI Layout
**การตัดสินใจ:**
- ใช้ `Canvas` + `Image` + `Button` + `CanvasGroup` ในการเรียงกันของร้านค้า  
- ใช้ `Canvas` 2 อันในการแบ่ง Section ต่างๆ เช่น หน้าเริ่มต้น กับหน้าร้านค้า  
- ดู Reference พวกในเกมบ้าง YouTube ส่วนตัวชอบเล่นเกมพวกลงดันเก็บเวลอยู่แล้ว
  ทั้งธีม หาจาก Freepik + AssetStore
- ใช้ `UIManager` ในการคุม UI บางส่วน

**เหตุผล:**
- รองรับขนาดจอและอัตราส่วนต่างๆ ของ Mobile  
- กด Button แล้วมี Transition โดยใช้ `DoTween` + `Canvas Group` ให้ Smooth  

---

## Systen
- `GameSaveManager`
- `SafeAreaFix`
- `ShopTutorialManager`
- `Shop`
- `ShopUIController`
- `ButtonEffect`
- `SoundManager`
- `SoundToggleButtons`
- `UIButtonSound`
- `UIManager`

## Item Card Design
**การตัดสินใจ:**
- Item ใช้วิธีโคลน Prefab แล้วเปลี่ยน `Image` โดยประกอบไปด้วย:  
  - Icon (`Image`)  
  - Item Name (`TextMeshPro`)  
  - Price (Coin)  
  - Buy & Sell Button  
  - Category (เช่น อาวุธ, น้ำยา, Buff)  

**เหตุผล:**
- โคลน Item ง่าย แค่เปลี่ยน Image ก็ใช้งานได้เลย  

---

## Problem & Solution
**Problem**
- Icon มีขนาดใหญ่เกิน  

**Solution**
- ปรับ Anchor Point + Canvas Scaler โดยใช้ `Match`  
- กำหนด Resolution ที่ `1080 x 1920`  

---

## Section Switching
**การตัดสินใจ:**
- ใช้ `Button` + `Image` แต่ละปุ่มมี Script Effect + Icon  
- Transition ใช้การ Fade Time  
- ใช้ `SoundManager` ควบคุมเสียงทั้งหมดในเกม (ปุ่ม, Music, SFX)  

**เหตุผล:**
- ง่ายต่อการเปลี่ยน Canvas และทำให้ UI ดูมีลูกเล่น  

---

## Responsive Layout
**การตัดสินใจ:**
- ใช้ `Canvas Scaler (Scale With Screen Size)` + Anchors สำหรับทุก UI element  
- ใช้ Script `SafeAreaFix`  

**เหตุผล:**
- รองรับมือถือหลายอัตราส่วน (16:9 และ 19.5:9)  
- ตรวจสอบแล้ว UI ไม่แตกหรือซ้อนกัน  
- ปุ่ม, Icon, Text ไม่เพี้ยน  

**Problem & Solution**
- ปุ่มเลื่อนผิดตำแหน่ง → ปรับแก้ด้วยการใช้ Anchor Point ตามต้องการ  

---

## Tools & Plugins
**การตัดสินใจ:**
- `DOTween` → ทำ UI Transitions ลื่นไหล  
- `TextMeshPro` → แสดงข้อความสวยงาม  
- `UniTask` → จัดลำดับขั้นตอนและ Logic ของ UI  

**เหตุผล:**
- ลดเวลาเขียนโค้ด Custom  
- เพิ่มประสิทธิภาพในการจัดการ UI  

---

## Download APK
👉 [Quest & Chest (v1.0)](https://drive.google.com/drive/folders/1EAmFNh1GXRaf9MKmpXPEz1ifZkidY8IR?usp=sharing)
