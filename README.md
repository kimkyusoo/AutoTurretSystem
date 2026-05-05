# 🎯 Unity 3D 포탑 사격 시스템 (Turret Shooting System)

> **Primitive Shape와 벡터 연산을 활용한 정밀 타겟 추적 및 발사 시뮬레이션**

본 프로젝트는 유니티의 계층 구조(Hierarchy)와 수학적 연산(`Atan2`, `RotateTowards`, `Angle`)을 이해하고, 이를 실전 포탑 시스템에 적용하는 것을 목표로 합니다.

---

## 🚀 주요 구현 기능

### 1. 환경 및 계층 구조 (Environment & Hierarchy)
- **도형 구성:** 바닥, 포탑 본체, 머리, 포신 등을 기본 도형(Cube, Capsule 등)으로 제작.
- **피벗 시스템:** 독립적인 회전을 위해 `YawPivot`(좌우)과 `PitchPivot`(상하)을 분리하여 설계.
- **발사점 설정:** 포신 끝에 `ShootPosition`을 배치하여 발사체의 생성 위치와 방향을 정의.

### 2. 타겟 드론 이동 (Target Drone)
- **회전 궤도:** `TargetPivot`을 부모로 하여 드론이 일정한 원형 경로를 따라 계속 이동.
- **실시간 추적:** 포탑이 드론의 위치를 실시간으로 계산하여 조준.

### 3. 정밀 조준 로직 (Tracking Logic)
- **Yaw (좌우):** 타겟의 높이를 무시한 수평 방향 벡터를 추출하여 부드럽게 회전.
- **Pitch (상하):** 타겟과의 거리 및 높이 차이를 `Mathf.Atan2`로 계산하여 상하 각도 조준.
- **각도 제한:** 포신이 과하게 꺾이지 않도록 `-45도`에서 `20도` 사이로 제한(`Mathf.Clamp`).

### 4. 사격 시스템 (Firing System)
- **조준 판정:** 포신의 정면(`forward`)과 타겟 방향의 각도 오차가 **5도 이내**일 때 사격 가능 상태로 전환.
- **프리팹 발사:** 프리팹 기반의 미사일이 생성되며, 포탑의 조준 방향과 일치하게 회전되어 발사.
- **자동 관리:** 생성된 미사일은 일정 시간 후 자동으로 파괴되어 메모리 효율성 확보.

---

## 🏗 하이어라키 구조 (Hierarchy)

```text
Turret (Empty)
└── Body (Cylinder)
└── YawPivot (Empty)  <-- TurretController 부착
    ├── Head (Cube)
    └── PitchPivot (Empty)
        ├── GunBarrel (Cube)
        └── ShootPosition (Empty) <-- 발사 위치

TargetSystem (Empty)
└── TargetPivot (Empty) <-- Orbit 스크립트 부착
    └── Drone (Sphere)
```
<img width="1150" height="604" alt="AutoTurret" src="https://github.com/user-attachments/assets/d34a0ee8-481a-4c7b-b776-4f599ec749d0" />

