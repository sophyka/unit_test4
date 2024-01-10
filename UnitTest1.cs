using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestLab4
{
    [TestClass]
    public class DatabaseManagerTests
    {
        private const string DbFilePath = "test_db.sqlite";

        [TestInitialize]
        public void Initialize()
        {
            using (DatabaseManager databaseManager = new DatabaseManager(DbFilePath))
            {
                databaseManager.ClearDatabase();
            }
        }

        [TestMethod]
        public void SaveTurtleData_ShouldInsertDataIntoDatabase()
        {
            TurtleStorage turtleData = new TurtleStorage
            {
                X = 10.5f,
                Y = 20.7f,
                Angle = 45,
                PenDown = true,
                PenColor = "Red",
                Dots = new List<Dot> { new Dot(1, 2), new Dot(3, 4) },
                Lines = new List<Line> { new Line(new Dot(1, 2), new Dot(3, 4), "Red") },
                Figures = new List<Figure> { new Figure() },
                CurrentFigure = new Figure(),
                Steps = new List<string> { "Step 1", "Step 2" }
            };

            using (DatabaseManager databaseManager = new DatabaseManager(DbFilePath))
            {
                databaseManager.SaveTurtleData(turtleData);

                string result = databaseManager.GetOneTurtle();
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void LoadTurtleData_ShouldReturnTurtleStorageFromDatabase()
        {
            TurtleStorage expectedData = new TurtleStorage
            {
                X = 15.3f,
                Y = 25.8f,
                Angle = 90,
                PenDown = false,
                PenColor = "Blue",
                Dots = new List<Dot> { new Dot(5, 6), new Dot(7, 8) },
                Lines = new List<Line> { new Line(new Dot(5, 6), new Dot(7, 8), "Blue") },
                Figures = new List<Figure> { new Figure() },
                CurrentFigure = new Figure(),
                Steps = new List<string> { "Step 3", "Step 4" }
            };

            using (DatabaseManager databaseManager = new DatabaseManager(DbFilePath))
            {
                databaseManager.SaveTurtleData(expectedData);

                TurtleStorage loadedData = databaseManager.LoadTurtleData();

                Assert.IsNotNull(loadedData);
                Assert.AreEqual(expectedData.X, loadedData.X);
                Assert.AreEqual(expectedData.Y, loadedData.Y);
                Assert.AreEqual(expectedData.Angle, loadedData.Angle);
                Assert.AreEqual(expectedData.PenDown, loadedData.PenDown);
            }
        }

    }
    [TestClass]
    public class DotTests
    {
        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenDotsAreEqual()
        {
            Dot dot1 = new Dot(1.5f, 2.5f);
            Dot dot2 = new Dot(1.5f, 2.5f);

            bool result = dot1.Equals(dot2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_WhenDotsAreNotEqual()
        {
            Dot dot1 = new Dot(1.5f, 2.5f);
            Dot dot2 = new Dot(3.0f, 4.0f);

            bool result = dot1.Equals(dot2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnSameHashCode_WhenDotsAreEqual()
        {
            Dot dot1 = new Dot(1.5f, 2.5f);
            Dot dot2 = new Dot(1.5f, 2.5f);

            int hashCode1 = dot1.GetHashCode();
            int hashCode2 = dot2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnDifferentHashCode_WhenDotsAreNotEqual()
        {
            Dot dot1 = new Dot(1.5f, 2.5f);
            Dot dot2 = new Dot(3.0f, 4.0f);

            int hashCode1 = dot1.GetHashCode();
            int hashCode2 = dot2.GetHashCode();

            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            Dot dot = new Dot(1.5f, 2.5f);

            string result = dot.ToString();

            Assert.AreEqual("(1.5;2.5)", result);
        }
    }
    [TestClass]
    public class FigureTests
    {
        [TestMethod]
        public void AddLine_ShouldAddLineToFigure()
        {
            Figure figure = new Figure();
            Line line = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");

            figure.AddLine(line);

            CollectionAssert.Contains(figure.Lines, line);
        }

        [TestMethod]
        public void Clear_ShouldRemoveAllLinesFromFigure()
        {
            Figure figure = new Figure();
            figure.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            figure.Clear();

            CollectionAssert.DoesNotContain(figure.Lines, new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
        }

        [TestMethod]
        public void GetLines_ShouldReturnAllLinesInFigure()
        {
            Figure figure = new Figure();
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            figure.AddLine(line1);
            figure.AddLine(line2);

            List<Line> result = figure.GetLines();

            CollectionAssert.Contains(result, line1);
            CollectionAssert.Contains(result, line2);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenFiguresAreEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure1.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            bool result = figure1.Equals(figure2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_WhenFiguresAreNotEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            bool result = figure1.Equals(figure2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnSameHashCode_WhenFiguresAreEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure1.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            int hashCode1 = figure1.GetHashCode();
            int hashCode2 = figure2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnDifferentHashCode_WhenFiguresAreNotEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            int hashCode1 = figure1.GetHashCode();
            int hashCode2 = figure2.GetHashCode();

            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void Display_ShouldPrintLinesInFigure()
        {
            Figure figure = new Figure();
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            figure.AddLine(line1);
            figure.AddLine(line2);

            var consoleOutput = TestHelpers.GetConsoleOutput(() => figure.Display());

            StringAssert.Contains(consoleOutput, line1.ToString());
            StringAssert.Contains(consoleOutput, line2.ToString());
        }
    }
    [TestClass]
    public class FigureTests
    {
        [TestMethod]
        public void AddLine_ShouldAddLineToFigure()
        {
            Figure figure = new Figure();
            Line line = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");

            figure.AddLine(line);

            CollectionAssert.Contains(figure.Lines, line);
        }

        [TestMethod]
        public void Clear_ShouldRemoveAllLinesFromFigure()
        {
            Figure figure = new Figure();
            figure.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            figure.Clear();

            CollectionAssert.DoesNotContain(figure.Lines, new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
        }

        [TestMethod]
        public void GetLines_ShouldReturnAllLinesInFigure()
        {
            Figure figure = new Figure();
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            figure.AddLine(line1);
            figure.AddLine(line2);

            List<Line> result = figure.GetLines();

            CollectionAssert.Contains(result, line1);
            CollectionAssert.Contains(result, line2);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenFiguresAreEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure1.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            bool result = figure1.Equals(figure2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_WhenFiguresAreNotEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            bool result = figure1.Equals(figure2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnSameHashCode_WhenFiguresAreEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure1.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            int hashCode1 = figure1.GetHashCode();
            int hashCode2 = figure2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnDifferentHashCode_WhenFiguresAreNotEqual()
        {
            Figure figure1 = new Figure();
            figure1.AddLine(new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black"));

            Figure figure2 = new Figure();
            figure2.AddLine(new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red"));

            int hashCode1 = figure1.GetHashCode();
            int hashCode2 = figure2.GetHashCode();

            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void Display_ShouldPrintLinesInFigure()
        {
            Figure figure = new Figure();
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            figure.AddLine(line1);
            figure.AddLine(line2);

            var consoleOutput = TestHelpers.GetConsoleOutput(() => figure.Display());

            StringAssert.Contains(consoleOutput, line1.ToString());
            StringAssert.Contains(consoleOutput, line2.ToString());
        }
    }
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenLinesAreEqual()
        {
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");

            bool result = line1.Equals(line2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_WhenLinesAreEqualInReverseOrder()
        {
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(3.0f, 4.0f), new Dot(1.0f, 2.0f), "Black");

            bool result = line1.Equals(line2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_WhenLinesAreNotEqual()
        {
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            bool result = line1.Equals(line2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnSameHashCode_WhenLinesAreEqual()
        {
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");

            int hashCode1 = line1.GetHashCode();
            int hashCode2 = line2.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnDifferentHashCode_WhenLinesAreNotEqual()
        {
            Line line1 = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");
            Line line2 = new Line(new Dot(5.0f, 6.0f), new Dot(7.0f, 8.0f), "Red");

            int hashCode1 = line1.GetHashCode();
            int hashCode2 = line2.GetHashCode();

            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            Line line = new Line(new Dot(1.0f, 2.0f), new Dot(3.0f, 4.0f), "Black");

            string result = line.ToString();

            StringAssert.Contains(result, "(  1:  2)--->(  3:  4) | Color: Black");
        }
    }
    [TestClass]
    public class TurtleTests
    {
        [TestMethod]
        public void ProcessCommand_ShouldChangePenState_WhenCommandIsPu()
        {
            Turtle turtle = new Turtle();

            turtle.ProcessCommand("pu");

            Assert.IsFalse(turtle.PenDown);
        }

        [TestMethod]
        public void ProcessCommand_ShouldChangePenState_WhenCommandIsPd()
        {
            Turtle turtle = new Turtle();

            turtle.ProcessCommand("pd");

            Assert.IsTrue(turtle.PenDown);
        }

        [TestMethod]
        public void ProcessCommand_ShouldChangeAngle_WhenCommandIsAngle()
        {
            Turtle turtle = new Turtle();

            turtle.ProcessCommand("angle 90");

            Assert.AreEqual(90, turtle.Angle);
        }

        [TestMethod]
        public void ProcessCommand_ShouldMoveTurtle_WhenCommandIsMove()
        {
            Turtle turtle = new Turtle();

            turtle.ProcessCommand("pd");
            turtle.ProcessCommand("move 10");

            Assert.AreEqual(10, turtle.X, 0.01);
            Assert.AreEqual(0, turtle.Y, 0.01);
        }

        [TestMethod]
        public void ProcessCommand_ShouldChangePenColor_WhenCommandIsColor()
        {
            Turtle turtle = new Turtle();

            turtle.ProcessCommand("color red");

            Assert.AreEqual("red", turtle.PenColor);
        }

        [TestMethod]
        public void ProcessCommand_ShouldDisplayFigures_WhenCommandIsListFigures()
        {
            Turtle turtle = new Turtle();
            turtle.ProcessCommand("pd");
            turtle.ProcessCommand("move 10");
            turtle.ProcessCommand("pu");

            turtle.ProcessCommand("list figures");

        }

        [TestMethod]
        public void IsIntersect_ShouldReturnTrue_WhenLinesIntersect()
        {
            Turtle turtle = new Turtle();
            Line line1 = new Line(new Dot(0, 0), new Dot(5, 5), "black");
            Line line2 = new Line(new Dot(0, 5), new Dot(5, 0), "black");

            bool result = turtle.IsIntersect(line1, line2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsIntersect_ShouldReturnFalse_WhenLinesDoNotIntersect()
        {
            Turtle turtle = new Turtle();
            Line line1 = new Line(new Dot(0, 0), new Dot(5, 5), "black");
            Line line2 = new Line(new Dot(6, 0), new Dot(10, 5), "black");

            bool result = turtle.IsIntersect(line1, line2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveToDatabase_ShouldSaveTurtleDataToDatabase()
        {
            Turtle turtle = new Turtle();
            turtle.ProcessCommand("pd");
            turtle.ProcessCommand("move 10");
            turtle.ProcessCommand("pu");

            turtle.SaveToDatabase();

        }

        [TestMethod]
        public void LoadTurtle_ShouldLoadTurtleDataFromDatabase()
        {
            Turtle turtle = new Turtle();
            turtle.ProcessCommand("pd");
            turtle.ProcessCommand("move 10");
            turtle.ProcessCommand("pu");
            turtle.SaveToDatabase();

            turtle.LoadTurtle();

        }
    }
    [TestClass]
    public class TurtleStorageTests
    {
        [TestMethod]
        public void TestProperties_SetAndGetProperties_ShouldMatch()
        {
            float expectedX = 10.5f;
            float expectedY = 20.8f;
            int expectedAngle = 45;
            bool expectedPenDown = true;
            string expectedPenColor = "blue";
            List<Dot> expectedDots = new List<Dot> { new Dot(1, 1), new Dot(2, 2) };
            List<Line> expectedLines = new List<Line> { new Line(new Dot(1, 1), new Dot(2, 2), "red") };
            List<Figure> expectedFigures = new List<Figure> { new Figure() };
            Figure expectedCurrentFigure = new Figure();
            ObservableCollection<string> expectedSteps = new ObservableCollection<string> { "Step 1", "Step 2" };

            TurtleStorage turtleStorage = new TurtleStorage
            {
                X = expectedX,
                Y = expectedY,
                Angle = expectedAngle,
                PenDown = expectedPenDown,
                PenColor = expectedPenColor,
                Dots = expectedDots,
                Lines = expectedLines,
                Figures = expectedFigures,
                CurrentFigure = expectedCurrentFigure,
                Steps = expectedSteps
            };

            Assert.AreEqual(expectedX, turtleStorage.X);
            Assert.AreEqual(expectedY, turtleStorage.Y);
            Assert.AreEqual(expectedAngle, turtleStorage.Angle);
            Assert.AreEqual(expectedPenDown, turtleStorage.PenDown);
            Assert.AreEqual(expectedPenColor, turtleStorage.PenColor);
            CollectionAssert.AreEqual(expectedDots, turtleStorage.Dots);
            CollectionAssert.AreEqual(expectedLines, turtleStorage.Lines);
            CollectionAssert.AreEqual(expectedFigures, turtleStorage.Figures);
            Assert.AreSame(expectedCurrentFigure, turtleStorage.CurrentFigure);
            CollectionAssert.AreEqual(expectedSteps, turtleStorage.Steps);
        }
    }
}
