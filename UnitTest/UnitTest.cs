using AOC;

namespace AOCUnitTest;

public class UnitTest
{
    [Fact]
    public void TestDay4()
    {
       Assert.True(Day4.isContained("10-20,15-20"));
       Assert.True(Day4.isContained("15-20,10-20"));
       Assert.False(Day4.isContained("10-12,15-20"));
       Assert.False(Day4.isContained("10-17,15-20"));

       Assert.True(Day4.isOverlap("1-10,1-5"));
       Assert.True(Day4.isOverlap("1-10,4-5"));
       Assert.True(Day4.isOverlap("1-10,4-10"));
       Assert.True(Day4.isOverlap("1-10,5-15"));
       Assert.True(Day4.isOverlap("1-10,10-15"));
       //in reverse
       Assert.True(Day4.isOverlap("1-5,1-10"));
       Assert.True(Day4.isOverlap("4-5,1-10"));
       Assert.True(Day4.isOverlap("4-10,1-10"));
       Assert.True(Day4.isOverlap("5-15,1-10"));
       Assert.True(Day4.isOverlap("10-15,1-10"));
    }
}