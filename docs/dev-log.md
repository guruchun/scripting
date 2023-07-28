## 요구사항
관리콘솔이나 시운전 프로그램, 테스트 자동화 프로그램에서 스크립트 실행기능을 내장한다.
* 스크립트의 형태는? C#, Python, VBS, PS1
* 스크립트를 바로 실행하나? 아니면 컴파일해서 바이너리를 실행하나?
* C# APP에서 스크립트를 실행하면, 스크립트는 APP과 상호작용하면서 기능을 수행해야 한다.
   - Script는 APP과 다른 프로세스로 실행될까? 그러면 데이터는 어떻게 공유할까?

## 접근방법
* Shell에서 Script를 실행할 수 있다면 APP에서도 Shell을 통해 Script를 실행하고 그 결과를 받을 수 있다.
   * VBS: VB Script로 COM Object를 다를 수 있음. 사용자 라이브러리도 COM Object로 생성해야함
   * PowerShell: 내장된 CmdLet으로 시스템을 자유롭게 제어할 수 있음. COM Object로 다룰 수 있음
* Python이나 C#으로 Script를 작성하고 Assembly 형태로 미리 Build해 놓고 호출한다.
   * Python: Python.NET
      - Python에서 .NET API를 호출하거나 C# APP에서 Python script를 실행할 수 있다.
   * C#: CodeDom과 Roslyn을 주로 사용한다.

## 방법1: PowerShell Cmdlet
### 요약
* 닷넷 라이브러리를 만든다.
* COM 호출 가능하도록 라이브러리(클래스)를 레지스트리에 등록한다.
* Script(VBScript, PS1)에서 Object 생성 후에 메소드를 호출한다.
   *  VBS: obj = CreateObject("ClassName") --> obj.Method()
   *  PS1: $obj = New-Object -TypeName ClassName --> obj.Method()
### 참고자료
* http://www.mvps.org/scripting/dotnet/index.htm
* https://learn.microsoft.com/en-us/powershell/scripting/learn/ps101/00-introduction
* https://learn.microsoft.com/en-us/powershell/scripting/samples/creating-.net-and-com-objects--new-object-

## 방법2: 런타임에 C# 코드를 단위 프로그램으로 생성/실행
### 참고자료
* Stackoverflow or other pages
   *  https://stackoverflow.com/questions/826398/is-it-possible-to-dynamically-compile-and-execute-c-sharp-code-fragments
   *  https://stackoverflow.com/questions/41404721/calling-external-application-method-from-c-sharp-script
* CodeDOM
   *  using System.CodeDom, System.CodeDom.Compiler
   *  https://www.c-sharpcorner.com/article/dynamically-creating-applications-using-system-codedom/
   *  https://www.sysnet.pe.kr/2/0/12809
   *  CodeDom으로 컴파일하는 것이 간단한 방법이라고 하나, .NET 6부터 지원하지 않는 것으로 보인다.
			§ https://learn.microsoft.com/en-us/dotnet/api/system.codedom.compiler.codedomprovider.compileassemblyfromsource
			§ 대신 Roslyn을 쓰도록 권장하는 듯
* Roslyn
   *  CodeAnalysis: 패키지를 추가로 설치해야 사용할 수 있다.
   *  using Microsoft.CodeAnalysis, Microsoft.CodeAnalysis.CSharp
		
	
