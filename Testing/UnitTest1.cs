using NUnit.Framework;
using API;

namespace Testing
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Get_KeyNotInExcel_NoKeyFound()
        {
            CRUD crud = new CRUD();
            var output = crud.get("Jimmy");
            Assert.That(output, Is.EqualTo("No key found"));
        }
        [Test]
        public void Get_KeyInExcel_ReturnValue()
        {
            CRUD crud = new CRUD();
            var output = crud.get("Apple");
            Assert.That(output, Is.EqualTo("fruit"));
        }
        [Test]
        public void Create_KeyExists_ReturnNull()
        {
            CRUD crud = new CRUD();
            crud.create("tea", "hotbeverage");
            Assert.That(crud.key, Is.EqualTo("tea"));
        }
        [Test]
        public void Update_Value_ReturnVoid()
        {
            CRUD crud = new CRUD();
            crud.update("apple", "fruit");
            Assert.That(crud.value, Is.EqualTo("fruit"));
        }
        [Test]
        public void Delete_Key_ReturnVoid()
        {
            CRUD crud = new CRUD();
            crud.Delete("tea");
            Assert.That(crud.key, Is.EqualTo("tea"));
        }
    }
}