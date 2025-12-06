using NUnit.Framework;
using System;

namespace MythicLegion.Tests
{
    [TestFixture]
    public class LegionTests
    {
        private Hero CreateTestHero(string name = "TestHero", int health = 100, int power = 50)
        {
           
            return new Hero(name, "TestType") { Health = health, Power = power, IsTrained = false };
        }

        [Test]
        public void Constructor_ShouldInitializeEmptyLegion()
        {
            var legion = new Legion();

            Assert.That(legion.GetLegionInfo(), Is.EqualTo("No heroes in the legion."));
        }

        [Test]
        public void AddHero_WithValidHero_ShouldAddSuccessfully()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Aragorn");

            legion.AddHero(hero);

            var info = legion.GetLegionInfo();
            Assert.That(info, Does.Contain("Aragorn"));
        }

        [Test]
        public void AddHero_WithNullHero_ShouldThrowArgumentNullException()
        {
            var legion = new Legion();

            var exception = Assert.Throws<ArgumentNullException>(() => legion.AddHero(null!));
            Assert.That(exception.ParamName, Is.EqualTo("hero"));
            Assert.That(exception.Message, Does.Contain("Hero cannot be null"));
        }

        [Test]
        public void AddHero_WithDuplicateName_ShouldThrowArgumentException()
        {
            var legion = new Legion();
            var hero1 = CreateTestHero("Gandalf");
            var hero2 = CreateTestHero("Gandalf");
            legion.AddHero(hero1);

            var exception = Assert.Throws<ArgumentException>(() => legion.AddHero(hero2));
            Assert.That(exception.Message, Does.Contain("Hero with name Gandalf already exists in the legion"));
        }

        [Test]
        public void AddHero_MultipleHeroesWithDifferentNames_ShouldAddAll()
        {
            var legion = new Legion();
            var hero1 = CreateTestHero("Legolas");
            var hero2 = CreateTestHero("Gimli");
            var hero3 = CreateTestHero("Frodo");

            legion.AddHero(hero1);
            legion.AddHero(hero2);
            legion.AddHero(hero3);

            var info = legion.GetLegionInfo();
            Assert.That(info, Does.Contain("Legolas"));
            Assert.That(info, Does.Contain("Gimli"));
            Assert.That(info, Does.Contain("Frodo"));
        }

        [Test]
        public void RemoveHero_WithExistingHero_ShouldReturnTrueAndRemoveHero()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Boromir");
            legion.AddHero(hero);

            var result = legion.RemoveHero("Boromir");

            Assert.That(result, Is.True);
            Assert.That(legion.GetLegionInfo(), Is.EqualTo("No heroes in the legion."));
        }

        [Test]
        public void RemoveHero_WithNonExistingHero_ShouldReturnFalse()
        {
            var legion = new Legion();

            var result = legion.RemoveHero("NonExistent");

            Assert.That(result, Is.False);
        }

        [Test]
        public void RemoveHero_FromLegionWithMultipleHeroes_ShouldRemoveOnlySpecifiedHero()
        {
            var legion = new Legion();
            legion.AddHero(CreateTestHero("Hero1"));
            legion.AddHero(CreateTestHero("Hero2"));
            legion.AddHero(CreateTestHero("Hero3"));

            var result = legion.RemoveHero("Hero2");

            Assert.That(result, Is.True);
            var info = legion.GetLegionInfo();
            Assert.That(info, Does.Contain("Hero1"));
            Assert.That(info, Does.Not.Contain("Hero2"));
            Assert.That(info, Does.Contain("Hero3"));
        }

        [Test]
        public void RemoveHero_WithNullOrEmptyName_ShouldReturnFalse()
        {
            var legion = new Legion();
            legion.AddHero(CreateTestHero("SomeHero"));

            Assert.That(legion.RemoveHero(null!), Is.False);
            Assert.That(legion.RemoveHero(string.Empty), Is.False);
        }

        [Test]
        public void TrainHero_WithExistingHero_ShouldIncreaseStatsAndReturnSuccessMessage()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Samwise", 80, 40);
            legion.AddHero(hero);

            var result = legion.TrainHero("Samwise");

            Assert.That(result, Is.EqualTo("Samwise has been trained."));
            Assert.That(hero.Health, Is.EqualTo(81));
            Assert.That(hero.Power, Is.EqualTo(50));
            Assert.That(hero.IsTrained, Is.True);
        }

        [Test]
        public void TrainHero_WithNonExistingHero_ShouldReturnNotFoundMessage()
        {
            var legion = new Legion();

            var result = legion.TrainHero("Ghost");

            Assert.That(result, Is.EqualTo("Hero with name Ghost not found."));
        }

        [Test]
        public void TrainHero_MultipleTimesOnSameHero_ShouldStackTraining()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Merry", 70, 30);
            legion.AddHero(hero);

            legion.TrainHero("Merry");
            legion.TrainHero("Merry");

            Assert.That(hero.Health, Is.EqualTo(72));
            Assert.That(hero.Power, Is.EqualTo(50));
            Assert.That(hero.IsTrained, Is.True);
        }

        [Test]
        public void TrainHero_WithNullOrEmptyName_ShouldReturnNotFoundMessage()
        {
            var legion = new Legion();

            Assert.That(legion.TrainHero(null!), Does.Contain("not found"));
            Assert.That(legion.TrainHero(string.Empty), Does.Contain("not found"));
        }

        [Test]
        public void GetLegionInfo_WithEmptyLegion_ShouldReturnNoHeroesMessage()
        {
            var legion = new Legion();

            var result = legion.GetLegionInfo();

            Assert.That(result, Is.EqualTo("No heroes in the legion."));
        }

        [Test]
        public void GetLegionInfo_WithSingleHero_ShouldReturnHeroInfo()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Pippin");
            legion.AddHero(hero);

            var result = legion.GetLegionInfo();

            Assert.That(result, Does.Contain("Pippin"));
            Assert.That(result, Does.Not.Contain(Environment.NewLine));
        }

        [Test]
        public void GetLegionInfo_WithMultipleHeroes_ShouldReturnAllHeroesOnSeparateLines()
        {
            var legion = new Legion();
            legion.AddHero(CreateTestHero("Hero1"));
            legion.AddHero(CreateTestHero("Hero2"));
            legion.AddHero(CreateTestHero("Hero3"));

            var result = legion.GetLegionInfo();

            Assert.That(result, Does.Contain("Hero1"));
            Assert.That(result, Does.Contain("Hero2"));
            Assert.That(result, Does.Contain("Hero3"));
            var lines = result.Split(Environment.NewLine);
            Assert.That(lines.Length, Is.EqualTo(3));
        }

        [Test]
        public void GetLegionInfo_AfterTraining_ShouldReflectUpdatedStats()
        {
            var legion = new Legion();
            var hero = CreateTestHero("Eowyn", 90, 60);
            legion.AddHero(hero);
            legion.TrainHero("Eowyn");

            var result = legion.GetLegionInfo();

            Assert.That(result, Does.Contain("Eowyn"));
        }

        [Test]
        public void CompleteWorkflow_AddTrainRemove_ShouldWorkCorrectly()
        {
            var legion = new Legion();
            var hero1 = CreateTestHero("Warrior", 100, 50);
            var hero2 = CreateTestHero("Mage", 80, 100);

            legion.AddHero(hero1);
            legion.AddHero(hero2);
            Assert.That(legion.GetLegionInfo(), Does.Contain("Warrior"));
            Assert.That(legion.GetLegionInfo(), Does.Contain("Mage"));

            var trainResult = legion.TrainHero("Warrior");
            Assert.That(trainResult, Is.EqualTo("Warrior has been trained."));
            Assert.That(hero1.Health, Is.EqualTo(101));
            Assert.That(hero1.Power, Is.EqualTo(60));

            var removeResult = legion.RemoveHero("Mage");
            Assert.That(removeResult, Is.True);
            Assert.That(legion.GetLegionInfo(), Does.Contain("Warrior"));
            Assert.That(legion.GetLegionInfo(), Does.Not.Contain("Mage"));
        }
    }
}