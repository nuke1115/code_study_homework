using hw3;
using System;
using System.Collections.Generic;

#nullable disable 

class Program
{

    static void Tf<T>(int? t) where T : struct
    {
        int? t2 = 10;
        string? s1 = null;
        
    }

    static void fun(Nullable<int> type)
    {
        int t = type.GetValueOrDefault();
        var f = Nullable.GetUnderlyingType;
    }

    static void Main()
    {
        ValueArrayList<int> list = new ValueArrayList<int>(16);

        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);

        foreach( var v in list)
        {
            Console.WriteLine(v);
        }

        List<int> l = new List<int>();
        l.Count;

    }
}



/*
 제네릭 제약 조건은 항상 마지막에

https://stackoverflow.com/questions/65746628/c-sharp-generics-where-clause-with-inheritance-and-interfaces
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#class-declarations

 */

/*
EqualityComparer<T>
 IEqualityComparer<T>를 구현하는 클래스로, 두 개체가 같은지 비교할 수 있는 메서드들을 정의한다


 EqualityComparer<T>.Default
두 원소를 비교하는 비교자
만약, IEquatable<T>를 구현한다면, 그걸 사용하고, 아니라면 Object에서 상속받은 Equals를 사용한다
https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default?view=net-8.0
 
 */

/*
 아니, 값을 안밀면 gc가 문제고, 값을 밀면 null이 문제고
 근데, 그렇다고 !연산자로 null경고를 끄자니 그건 또 위험할거같고
 */

/*
https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references#generics
//nullable reference types - 제네릭에 넣어주는 인수가 값인지 참조인지에 따라 T?가 nullable이 될수도 안될수도 있다
https://learn.microsoft.com/ko-kr/dotnet/csharp/language-reference/operators/null-forgiving


null에서의 ? 연산자 : 이 변수는 null을 가질 수 있음을 명시적으로 알려줌
null에서의 ! 연산자 : 이 식에 대해 null관련 검사를 생략한다

null관련한 !연산자:
이 변수에 대한 null 경고를 끈다.
주의점:null관련 경고를 끄기 때문에, null에 대해 특히 더 신경을 써야 한다

이 자료구조 구현에 굳이 ?와 !를 같이 쓰는 이유:
내부에서 참조타입일 때, 값을 초기화해서 gc가 해당 원소를 수집하게 하기 위해 ?를 배열 선언 시 원소부분에 넣고
!는 ?를 써서 get함수에서 리턴값으로 원소를 넘길 때, 오류가 발생하는것을 끄기 위해서 넣음.
근데, 이런 사용의 경우 null검사를 생략한다는 위험성이 있지만, 다음 이유들에 의해 정당화됨.

?:
1: nullable reference types에 의해 값타입일때는 ?가 없는 상태로 사용되고, 참조타입일때만 ?붙은채로 사용됨
2: default연산자로 값 초기화에 사용될 값을 가져올 때, 참조타입은 null이 반환되고, 일반 T[]에서는 경고가 발생하기 떄문

!: 이건 인덱서의 get에만 한정적으로 쓰임
쓰이는 이유:
만약 값타입이 제네릭 인자로 올 떄: 값타입은 null인 경우가 없으니 괜찮음
만약, 제네릭 인자로 ?를 붙인 타입이 올 때: 사용자가 직접 null을 넣겠다고 한것이니, 경고가 안떠도 괜찮음
만약, 일반 참조타입이 올 떄 : 사용자가 직접 null을 넣지 않는 이상, null인 경우가 없으니, 경고가 안떠도 괜찮음
그리고, 기본적으로 사용자가 null을 직접 원소로 넣는 경우는, 내부가 아닌 사용자가 자신이 null을 넣은것에 대한 책임을 지는 경우기에, 안떠도 괜찮음
그리고, 내부에서 직접 값에 역참조를 하는 일은 없으니, 안떠도 괜찮음



throw예외 관련 추가 공부
제네릭 관련 추가 공부

오후2:11~

만들어야될것

스택,큐,리스트(배열,연결) + 추가(dict)


값에는 참조와 값 두가지 종류가 있다.
값은 그냥 있어도 상관 없는데, 참조는 참조를 들고있으면 gc가 수집 안함(메모리 낭비)
그레서, 참조형을 담는 경우는 그 값을 지울 때 참조도 같이 지워야 한다
그레서 참조형과 그게 아닌걸 일반적인 값형식하고 같이 다루기에는 약간 무리가 있다

그렇다면, 값형식만 담는거하고 참조형식만 담는거하고 나눠볼까?
공통되는 부분들을 추상클래스로 빼고, 그걸 상속받는걸 만들까?

~오후2:17

오전 10시 20분
11:30

8:40
*/