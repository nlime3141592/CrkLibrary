using System;
using System.IO;
using System.Text.Json;

#nullable enable

namespace CRK2
{
    public enum ItemType : int
    {
        Wood, Jellybean, Sugar, BiscuitPowder, Jellyberry, Milk, CottonCandy, // 기본 재료
        Axe, Pickaxe, Saw, Shovel, Pile, Tongs, SugarHammer, // 뚝딱 대장간
        JellybeanJam, SweetberryJam, DalgonaJam, PomegranateJam, PopberryJam, // 설탕몽땅 잼가게
        PineconeDoll, AcornLamp, CuckooClock, Dreamcatcher, // 롤케이크 공작소
        RyeBread, Jampie, Focaccia, Donut, Castella, Croissant, // 갓 구운 빵집
        JellyStew, JellyBugger, CandyCreamPasta, FluffyOmurice, PizzaJelly, JellybeanMeal, // 잼파이 레스토랑
        Flowerpot, GlassPlate, Marble, DessertBowl, // 토닥토닥 도예공방
        CandyFlower, HappyFlower, CandyBouquet, FlowerBasket, GlassBouquet, YogurtWreath, // 행복한 꽃가게
        Cream, Butter, HandyCheese, // 밀키 우유 가공소
        JellybeanLatte, BubbleTea, SweetberryAde, // 카페 라떼
        CandyCushion, BearCottonDoll, DragonDoll, // 러블리 인형공방
        CreamRootBeer, RedberryJuice, Wildbottle, // 오크통 쉼터
        Muffin, StrawberryCake, ChiffonCake, // 퐁 드 파티세리
        GlazedRing, Brooch, JellyCrown // 살롱 드 쥬얼리
    }

    class Program
    {
        static void Main(string[] args)
        {
            CrkManager.Start();
        }

        static void Pause(string message)
        {
            Console.WriteLine(message ?? "아무 키나 입력하세요..");
            Console.ReadKey();
        }
    }
}