# Refugio
5년전 개인 포트폴리오, 마켓에 제출했던 프로젝트.<br>
AI 의 발전으로 이 수준의 게임은 누구나 쉽게 만들 수 있으므로 공개 전환.
오래 된 프로젝트로 방해기믹의 설명이 정확하지 않습니다.

# 개요
- Fluffy 를 베이스로 만든 게임입니다.
- Tile 기반의 여러 맵을 전환하며 유저를 방해하는 다양한 기믹이 있습니다

# 사용 기술스택
- Unity 2019.4.0f1
- Unity TileMap
- Google Play Game Services
- Google Mobile Ads

# 등장 방해 기믹
- Dark Woods
  - Slug (SlugBubble 생성하여 진행방해)
  - Opossum (자신의 중력 변경 및 이동)
  - Frog (Random.Y 만큼의 위치를 점프하며 FrogBomb 을 경로에 생성)

- Dried Depth
  - Alkanoid (두 알카노이드가 공을 교환하며 진로 방해)
  - Bee (전방위 일정한 거리로 랜덤한 시간마다 이동)
  - Guardian (일정거리 내 플레이어를 자신의 쪽으로 끌어들임)
  - Jumper (특정 범위 내에서 랜덤하게 이동하며 점프)
  - Octopus (Lair 에서 생성되며 Vector.left 방향으로 탄을 발사)
  - WaterFall (맵 기믹, 맵 자체에 붙어있으며 플레이어를 아랫방향으로 끌어내림)
  - StoneMan (본체를 중심으로 방해 위성이 회전함)

- Memory Retro
  - FallDownBlocks (낙하 오브젝트를 다수 생성)
  - FloatingPlatform (공중을 수직, 수평으로 이동하는 방해 오브젝트)
  - Cannon
  - Bubble (일정 범위 내에서 점프와 이동을 반복하는 소형 오브젝트)
 
- Nature
  - Eagle (지정한 범위 내 각도에서 일정 시간마다 대시)
  - Gluplant
 
- Underwater
  - BigFish (랜덤한 각도로 이동)
  - Crab (낮은 속도로 이동하며 탄을 발사, 지상 유닛)
  - DartFish (플레이어가 접근하기 전까지 매우 빠른 속도로 날아오는 물고기 생성)
  - Mine (어뢰)
 
# 향후 작업
- 에디터 내 실행가능하게 수정
  - GPGS, Google Mobile Ads 로 인한 프로젝트 실행이 안되고있음 (수정 예정)
