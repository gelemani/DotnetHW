using System.Collections.Generic;
using System;
using task4._4.Models;

namespace task4._4.ViewModels;

public interface ICreateEntityFactory
{
    List<BlogEntity> CreateBlogEntities();
    List<NewsEntity> CreateNewsEntities();
}


public class CreateEntityFactory : ICreateEntityFactory
{
    public List<BlogEntity> CreateBlogEntities()
    {
        return new List<BlogEntity>()
        {
            CreateBlogEntity(
            "Коротко о главном",
            "Were dinosaurs warm-blooded like birds and mammals or cold-blooded like reptiles? It’s one of paleontology’s oldest questions, and gleaning the answer matters because it illuminates how the prehistoric creatures may have lived and behaved.\r\n\r\nChallenging the prevailing idea that they were all slow, lumbering lizards that basked in the sun to regulate their body temperature, research over the past three decades has revealed that some dinosaurs were likely birdlike, with feathers and perhaps the ability to generate their own body heat.\r\n\r\nHowever, it’s hard to find evidence that unquestionably shows what dinosaur metabolisms were like. Clues from dinosaur eggshells and bones have suggested that some dinosaurs were warm-blooded and others were not.\r\n\r\nA new study published in the journal Current Biology on Wednesday suggested that three main dinosaur groups adapted differently to changes in temperature, with the ability to regulate body temperature evolving in the early Jurassic Period about 180 million years ago.",
            "petya.jpg",
            new List<string> {"Tag1", "Tag2", "Tag3", "Tag 4"}
            )
        };
    }


    public List<NewsEntity> CreateNewsEntities()
    {
        return new List<NewsEntity>()
        {
            CreateNewsEntity("Запасы нефти в США за неделю снизились на 2,5 млн баррелей", DateTime.Now),
            CreateNewsEntity("ФРС оставила ключевую ставку без изменений", DateTime.Now.AddDays(-3)),
            CreateNewsEntity("РФ опустилась на восьмое место по объему валютных резервов", DateTime.Now.AddDays(-10))
        };
    }
    
    private BlogEntity CreateBlogEntity(string article, string text, string imagePath, List<string> tags)
    {
        return new BlogEntity
        {
            Article = article,
            Text = text,
            ImagePath = imagePath,
            Tags = tags
        };
    }

    private NewsEntity CreateNewsEntity(string text, DateTime publishTime)
    {
        return new NewsEntity
        {
            Text = text,
            PublishTime = publishTime
        };
    }
}


public class MainWindowViewModel : ViewModelBase
{
    private readonly ICreateEntityFactory _createEntityFactory;

    public MainWindowViewModel()
    {
        BlogEntities = _createEntityFactory.CreateBlogEntities();
        NewsEntities = _createEntityFactory.CreateNewsEntities();
    }

    public List<BlogEntity> BlogEntities { get; set; }
    public List<NewsEntity> NewsEntities { get; set; }

}