using Library.Models.API.UserMessaging;
using Microsoft.Extensions.DependencyInjection;
using Service.Dtos.Message;
using Service.Models.Message;
using Service.Repositories.Interfaces;
using Service.Services.Interfaces.MessageProcessing;

namespace Service.Services.Implementations.MessageProcessing
{
    public class SubmittedPlayerInvitationProcessor(IServiceScopeFactory serviceScopeFactory) : SubmittedMessageAbstractProcessor
    {
        protected override AbstractMessage<PlayerInvitationDto> CreateMessage(ProfileMessageSubmitted submittedMessage)
        {
            throw new NotImplementedException();
        }
    }
    //PublishedMessageProcessor - чисто для получения, создания сущности и сохранение в бд и отправки по сигналр на фронт - фабрика
    //messageinteractionprocessor - синглтон со списком  классов сообщений с локами для взаимодействия с сообщениями - если приходит команда на обработку сообщения, создаётся класс сообщения и сообщение лочится для взаимодействия пока оно не завершится,
    //если есть на очереди взаимодействие от другого пользователя - меняется поведения метода в зависимости от текущего состояния сообщения (Вроде это паттерн кстати, стейт машина что ли). плюс тут же можно отправлять обновление по сообщениям по сигналр (то есть ответы по сигналр не только
    //про создание новые собщения для получателя, а для всех онлайн участников "переговоров". Если все взаимодействия с сообщением завершены, то класс "закрывается" и сообщение уходит в бд
    //плсю возможно синглтон для бэкграунд очистки сообщений в состоянии expired или rescinded
    //кстати ответы на сообщения можно производить чисто по сигналр - тут рест по-момеу вообще не нужен - ты на сайте - будь добр 
    // в teamplayerprofiles acceptplayer и exitteam методы для взаимодейсвтия
}
