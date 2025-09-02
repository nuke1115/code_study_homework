namespace Homwork3
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(default(Program) is null);
            Program p = null;
            
        }
    }
}

/*

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

*/