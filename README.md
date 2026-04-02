# Pascal

![pages-light](https://github.com/hyngng/pascal.drill/blob/main/Image/pages-light.png)

.NET 9 및 WinUI 3 기반 Windows PDF 유틸리티 앱입니다. MVVM 아키텍처와 DI 패턴으로 설계되었습니다.

## 주요 기능

- **PDF 병합** — 여러 파일을 순서대로 배치해 하나로 합칩니다.
- **PDF 분할** — 페이지 범위(`1-3, 5, 7`) 또는 단위 크기로 분할합니다.
- **Labs** — 실험적 기능을 토글할 수 있는 설정 공간입니다.

## 기술 스택

| 항목 | 내용 |
|--|--|
| Framework | .NET 9 / WinUI 3 (Windows App SDK) |
| Pattern | MVVM (`CommunityToolkit.Mvvm`, DI) |
| PDF 처리 | PDFsharp, PdfPig |
