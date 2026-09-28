# SG

Windows용 .NET Framework 4.8 WPF 프로세스 관리 도구.

## 구현
- 실행 중 프로세스 목록 조회
- PID/이름 필터
- 메모리 사용량 및 x86/x64 표시
- 목록 새로고침
- 선택 프로세스에 대한 정상 종료 요청
- 로컬 사용자 설정 저장

## 제외
원본 Debug 빌드에서 확인된 DLL Injection, 강제 TerminateProcess, SuspendThread/ResumeThread 기반 제어, 보안 제품 우회/탐지 회피 및 커널 드라이버 기능은 구현하지 않는다.

## 빌드
Visual Studio 2022 + .NET Framework 4.8 Developer Pack에서 SG.sln을 빌드한다.
