using FluentAssertions;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;

namespace GRSMU.Core.Tests.Gradebooks.Parsers
{
    public class Tests
    {
        private string RawPage { get; set; }
        private GradebookParser Parser { get; set; }
        
        [SetUp]
        public async Task Setup()
        {
            Parser = new GradebookParser();

            await using (var stream = new FileStream("Data/GradebookPage.txt", FileMode.Open))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    RawPage = await streamReader.ReadToEndAsync();
                }
            }
        }

        [Test]
        public async Task GetStudentFullNameTest()
        {
            var result = await Parser.ParseAsync(RawPage);

            result.Should().NotBeNull();

            result.StudentFullName.Should().BeEquivalentTo("��������� ����");
        }

        [Test]
        public async Task GetDisciplinesTest()
        {
            var result = await Parser.ParseAsync(RawPage);

            result.Should().NotBeNull();

            result.Result.Should().NotBeNull();
            result.Result.Disciplines.Should().NotBeNullOrEmpty();

            var actualDisciplines = result.Result.Disciplines.Select(x => x.Name).ToList();
            var expectedDisciplines = new List<string>
            {
                "����������� �������� � ����� ��������",
                "����������� ���� (����)",
                "��������� ����",
                "�������� ��������",
                "����������� �����",
                "����������� � �������� (��������� ���)",
                "������ ������",
                "����������� ����: ���������������� �������",
                "���������� ��������",
                "��������������� ��������",
                "����������",
                "����������� �������������"
            };

            actualDisciplines.Should().ContainInOrder(expectedDisciplines);
        }
    }
}