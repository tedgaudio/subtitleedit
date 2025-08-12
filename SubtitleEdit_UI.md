🖥️ Subtitle Edit UI 구성 분석
�� 전체 UI 구조
1. 메인 윈도우 레이아웃
```
┌─────────────────────────────────────────────────────────────┐
│                    MenuStrip (메뉴바)                        │
├─────────────────────────────────────────────────────────────┤
│                 ToolStrip (툴바)                             │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────────┐  ┌─────────────────────────────────┐  │
│  │                  │  │                                 │  │
│  │Subtitle ListView │  │         Video Player            │  │
│  │ (자막 목록)       │  │        (비디오 플레이어)           │  │
│  │                 │  │                                  │  │
│  ├─────────────────┤  ├──────────────────────────────────┤  │
│  │                 │  │                                  │  │
│  │   Edit Panel    │  │             Waveform             │  │      
│  │    (편집 패널)   │  │               (파형)              │  │
│  │                 │  │                                  │  │
│  └─────────────────┘  └──────────────────────────────────┘  │
│                                                             │
├─────────────────────────────────────────────────────────────┤
│                 StatusStrip (상태바)                         │
└─────────────────────────────────────────────────────────────┘
```
🎛️ 주요 UI 컴포넌트
1. 상단 컨트롤
- MenuStrip: 파일, 편집, 도구, 비디오, 동기화, 옵션, 네트워킹, 도움말
- ToolStrip: 자주 사용하는 기능들의 빠른 접근 버튼들
  - 파일 관리 (새로 만들기, 열기, 저장)
  - 찾기/바꾸기
  - 오류 수정, 맞춤법 검사
  - 레이아웃 변경, 소스 뷰 전환

2. 메인 작업 영역
왼쪽 패널 (자막 편집 영역):
- SubtitleListView: 자막 목록을 표시하는 그리드 뷰
  - 컬럼: 번호, 시작시간, 종료시간, 지속시간, 텍스트, 배우, 스타일 등
  - 문법 강조, 실시간 편집 지원  
- Edit Panel (groupBoxEdit): 선택된 자막 편집
  - 텍스트 편집기
  - 시간 코드 조정
  - 스타일, 배우, 메타데이터 설정

오른쪽 패널 (미디어 영역):
- Video Player: 비디오 재생 및 동기화
- Waveform: 오디오 파형 표시 및 편집

🎨 레이아웃 관리 시스템
12가지 레이아웃 옵션
```csharp
public static class LayoutManager {
    public const int LayoutNoVideo = 11;
    
    // 레이아웃 0-10: 비디오 포함 다양한 배치
    // 레이아웃 11: 비디오 없음 (자막 편집 전용)
}
```
주요 레이아웃:
- Layout 0: 기본 (비디오 오른쪽)
- Layout 1: 비디오 왼쪽
- Layout 2: 비디오 위쪽
- Layout 3: 비디오 아래쪽
- Layout 11: 비디오 없음

🔧 핵심 UI 컨트롤들
1. SubtitleListView (자막 목록)
```csharp
public sealed class SubtitleListView : ListView {
    // 컬럼 정의
    public enum SubtitleColumn {
        Number, Start, End, Duration, 
        CharactersPerSeconds, WordsPerMinute, Gap,
        Actor, Region, OnOffScreen, Diegetic, 
        DFX, DialogueReverb, Notes, Text, 
        TextOriginal, Extra, Network
    }
    
    // 기능
    public bool UseSyntaxColoring { get; set; }
    public string SubtitleFontName { get; set; }
    public int SubtitleFontSize { get; set; }
}
```
특징:
- 문법 강조: 자막 텍스트의 문법 오류 실시간 표시
- 다중 선택: 여러 자막 동시 편집
- 드래그 앤 드롭: 자막 순서 변경
- 컨텍스트 메뉴: 우클릭으로 빠른 편집

2. Edit Panel (편집 패널)

```csharp
// 편집 패널 구성 요소들
private GroupBox groupBoxEdit;
private TextBox textBoxListViewText;      // 자막 텍스트 편집
private NikseTimeUpDown timeUpDownStart;  // 시작 시간
private NikseTimeUpDown timeUpDownEnd;    // 종료 시간
private ComboBox comboBoxEditActor;       // 배우 설정
private ComboBox comboBoxEditStyle;       // 스타일 설정
```

3. Video Player Container

```
public class VideoPlayerContainer : UserControl {
    // 비디오 재생, 일시정지, 탐색
    // 프레임 단위 이동
    // 오디오 트랙 선택
    // 자막 동기화 도구
}
```csharp

4. Audio Visualizer (파형 표시)
```
public class AudioVisualizer : UserControl {
    // 오디오 파형 시각화
    // 자막 타이밍 조정
    // 음성 인식 지원
}
```

🎯 UI 상호작용 패턴

1. 자막 편집 워크플로우

```
1. SubtitleListView에서 자막 선택
2. Edit Panel에서 텍스트/시간 수정
3. Video Player에서 실시간 미리보기
4. Waveform에서 정확한 타이밍 조정
```
2. 실시간 동기화
- 비디오 재생 중: 현재 자막 하이라이트
- 파형 클릭: 해당 시간으로 자막 이동
- 텍스트 편집: 즉시 미리보기 반영

3. 다중 뷰 지원
- ListView 모드: 그리드 형태로 자막 목록
- Source View 모드: 원본 텍스트 편집
- 번역 모드: 원본과 번역 동시 편집

🎨 테마 및 스타일링
다크/라이트 테마 지원
- DefaultTheme: 기본 밝은 테마
- DarkTheme: 어두운 테마
- Black: 검은색 테마
- Legacy: 레거시 테마

다국어 지원
- 40개 이상 언어 파일
- RTL (오른쪽에서 왼쪽) 언어 지원
- 유니코드 문자 입력 지원

�� 고급 UI 기능
1. 네트워킹 협업
- 실시간 다중 사용자 편집
- 채팅 기능
- 세션 관리

2. 플러그인 시스템
- 확장 가능한 UI
- 커스텀 도구 추가

3. 접근성
- 키보드 단축키
- 스크린 리더 지원
- 고대비 모드
