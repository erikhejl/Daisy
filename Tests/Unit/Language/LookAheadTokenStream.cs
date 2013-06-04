namespace Ancestry.Daisy.Tests.Unit.Language
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Language;

    using NUnit.Framework;

    public class LookAheadTokenStream
    {
        [TestCase("a",1,true,'a')]
        [TestCase("",1,false,(char)0)]
        [TestCase("abcde",2,true,'b')]
        [TestCase("abcde",3,true,'c')]
        [TestCase("abcde",4,true,'d')]
        [TestCase("abcde",5,true,'e')]
        [TestCase("abcde",6,false,(char)0)]
        [TestCase("abcde",7,false,(char)0)]
        [TestCase("abcde",int.MaxValue,false,(char)0)]
        public void ItDoesLookahead(string input, int count, bool eHas, char eChar)
        {
            var result =  new LookAheadStream<char>(input).LookAhead(count);
            Assert.AreEqual(eHas, result.HasCharactersUpTo);
            Assert.AreEqual(eChar, result.Value);
        }

        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abcdefghi a adf ad;lfjasd asdfl;kjsdf")]
        [TestCase("")]
        public void ItEnumerates(string @in)
        {
            Assert.That(new LookAheadStream<char>(@in).SequenceEqual(@in));
        }

        [TestCase("abcd",1)]
        [TestCase("a",1)]
        [TestCase("ab",4)]
        [TestCase("abcdefghi a adf ad;lfjasd asdfl;kjsdf",3)]
        [TestCase("",3)]
        public void ItEnumeratesAfterLookAhead(string @in, int lookTo)
        {
            var lookAhead = new LookAheadStream<char>(@in);
            lookAhead.LookAhead(lookTo);
            var enumeratedLookAhead = lookAhead.ToArray();
            Assert.That(enumeratedLookAhead.SequenceEqual(@in));
        }

        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [TestCase("")]
        [TestCase("b")]
        [TestCase("Lorem Ipsum is simply dummy text of the printing and typesetting industry.")]
        public void ATotallyCrazyLookAheadDoesNotHurtTraversal(string @in)
        {
            var rand = new Random();
            var lookAhead = new LookAheadStream<char>(@in);
            foreach (char t in @in)
            {
                lookAhead.LookAhead(rand.Next(10));
                Assert.IsTrue(lookAhead.MoveNext());
                Assert.AreEqual(t, lookAhead.Current);
            }
        }
    }
}
