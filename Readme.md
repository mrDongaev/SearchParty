## ЗАПУСК ПРИЛОЖЕНИЯ

Требуется Docker с Linux контейнерами

### Миграция БД

Создание файлов миграции и обновление БД для каждого отдельного сервиса контроллируется переменными среды в ```SearchParty/Docker/migrations/.env```:

- ```НАЗВАНИЕ_СЕРВИСА__ADD_MIGRATION```  

- ```НАЗВАНИЕ_СЕРВИСА__APPLY_MIGRATION```  

- ```НАЗВАНИЕ_СЕРВИСА__MIGRATION_NAME```  

#### Для исполнения миграций:  

1. Открыть в CLI ```путь_до_проекта/SearchParty/Docker/migrations/```
1. Исполнить команду ```docker-compose up```
1. Дождаться полного выполнения миграций (контейнеры с проектами должны самостоятельно завершить работу без ошибок, должны появится заполненные volume для всех сервисов)
1. Исполнить команду ```docker-compose down --rmi local``` (контейнеры и образы после миграций не нужны, если не нужно, чтобы они занимали место, volumes при этом остаются)

### Запуск

Заполнение БД тестовыми данными контроллируется переменными среды в ```Search/Docker/run/.env``` по имени ```НАЗВАНИЕ_СЕРВИСА__SEED_DATABASE```

1. Открыть в CLI ```путь_до_проекта/SearchParty/Docker/run```
1. Исполнить команду ```docker-compose up```
1. Дождаться полного запуска приложения
1. Фронтэнд-клиент станет доступен по адресу ```http://localhost:3000```

## Приложение для нахождения группы для рейтинговой игры в Дота 2.

В Дота 2 существует рейтинговый режим игры с общим рейтингом игроков (MMR, matchmaking rating).
Проблема нахождения заключается в том, что в команде существует 5 ролей (керри, мидер, оффлейн, саппорт и роумер) и пересекаться они не должны,
большинство игроков играет в основном на 1-2 ролях. Также у игроков обычно есть свой набор героев, 
на которых они комфортно себя чувствуют (из 120 героев, доступных в Доте), что также важно при подборе группы. 
Также, при групповой игре с рейтингом для баланса игры не допускается иметь слишком большой разброс между игроками с самым меньшим и самым большим рейтингом
Также, нельзя играть в рейтинг в группе из 4 человек, это может быть группа из 2, 3 или 5 человек. Матч в таких случаях найти нельзя.

### Отсюда возникла идея создать сервис, в котором игроки смогут следующее:
1. Создать свой профиль, в котором игрок сможет указать свой текущий рейтинг.
1. Свой профиль можно будет выставлять в общую доску поиска игроков, с помощью которой уже созданные группы смогут добирать себе игроков.  
1. Отсюда, игроки также смогут создавать группы, которые изначально могут наполнять уже имеющимися игроками.
1. Одиночные игроки смогут смотреть общую доску групп и проситься на включение в группу, если она им подходит.
1. Нужен способ взаимодействия игроков на платформе, в моём представлении это должно происходить с помощью чата между игроками внутри платформы.
1. Игроки могут сохранять у себя в профиле шаблоны команд и шаблоны своих одиночных профилей (например для разных ролей и героев), и выставлять те или иные шаблоны общую доску.
1. Общие доски как одиночных игроков, так и групп должны иметь соответствующие возможности фильтрации и сортировки для нахождения подходящей группы/игрока согласно выше приведённым критериям.
1. Возможна интеграция со Steam API для реализации логина через их JWT токен и автоматическое стягивание информации об MMR игрока для автозаполнения профиля.

### Ссылка на доску задач:
https://ru.yougile.com/team/9192842eeb1a/C%23-Professional/Найти-Пати

#### Архитектура приложения
![Architecture](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/app_schema_v2.png)

#### Схема БД сервиса профилей игроков и команд
![schema](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/profile_and_teams_schema_v2.png)

#### Сценарии использования системы авторизации и профиля пользователя
![use-case](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/auth_use_case.png)

#### Сценарии использования управлением собственными профилями команд и игроков
![use-case](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/profiles_use_case.png)

#### Сценарии использовании системы просмотра досок профилей команд и игроков
![use-case](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/boards_use_case.png)

#### Сценарии использовании системы взаимодействия с другими пользователями
![use-case](https://raw.githubusercontent.com/mrDongaev/SearchParty/dev/docs/user_interaction_use_case.png)
