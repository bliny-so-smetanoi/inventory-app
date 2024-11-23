import i18next from "i18next";
import {initReactI18next} from "react-i18next";

const resourses = {
    ru: {
        translation: {
            'Welcome to inventory system!': 'Добро пожаловать в систему инвентаризации!',
            'Welcome!': 'Добро пожаловать!',
            'Log In': 'Войти',
            'Logout': 'Выход',
            'delete': 'Удалить',
            'edit': 'Редактировать',
            'Name': 'Наименование',
            'Mediafiles': 'Медиафайлы',
            'Description': 'Описание',
            'No categories...': 'Нет категорий...',
            'view items': 'Просмотр инвентаря',
            'No classes...': 'Нет классов...',
            'Condition': 'Состояние',
            'Barcode': 'Баркод предмета',
            'Category name': 'Название категории',
            'No items...': 'Никаких предметов',
            'Email': 'Почта',
            'Full name': 'Полное имя',
            'Access level': 'Уровень доступа',
            'No users...': 'Никаких пользователей',
            'About Us': 'О нас',
            'Eng': 'Англ',
            'Rus': 'Рус',
            'Show users': 'Показать пользователей',
            'List classrooms': 'Список кабинетов',
            'Show categories': 'Показать категории',
            'Choose category': 'Выбрать категорию',
            'Login': 'Войти',
            'Email:': 'Почта:',
            'Password:': 'Пароль:',
            'Password': 'Пароль',
            'Back': 'Назад',
            'Add category': 'Добавить категорию',
            'Category was created!': 'Категория создана!',
            'Save changes': 'Сохранить изменения',
            'Loading...': 'Загрузка...',
            'Create category': 'Создать категорию',
            'Add classroom': 'Добавить кабинет',
            'Classroom was created!': 'Кабинет создан!',
            'Category was edited!': 'Категория была изменена!',
            'Create class': 'Создать кабинет',
            'No access for this functionality!': 'Нет доступа для этой функции!',
            'No auth': 'Нет авторизации',
            'Item number': 'Номер предмета',
            'Categories': 'Категории',
            'Add item': 'Добавить предмет',
            'Item was created!': 'Предмет добавлен!',
            'Classroom number': 'Номер кабинета',
            'Item was edited!': 'Предмет был отредактирован!',
            'Role:': 'Роль:',
            'Admin': 'Администратор',
            'Moderator': 'Модератор',
            'Add user': 'Добавить пользователя',
            'Add User': 'Добавить пользователя',
            'User was created!': 'Пользователь создан!',
            'Create account': 'Создать Аккаунт',
            'The mission of Astana IT University is to provide digital transformation through training, research and successful innovation.': 'Миссия Астанинского ИТ-университета - обеспечить цифровую трансформацию за счет обучения, исследований и успешных инноваций.',
            'Vision. Astana IT University is a leading center of competence for digital transformation in Central Asia.': 'Видение. Астанинский ИТ-университет является ведущим центром компетенций по цифровой трансформации в Центральной Азии.',
            'The global goal is to train highly qualified specialists in the digital economy based on interdisciplinary technologies.': 'Глобальная цель - подготовка высококвалифицированных специалистов в цифровой экономике на основе междисциплинарных технологий.',
            'Phone': 'Номер Телефона',
            'All': 'Все',
            'Available': 'Доступно',
            'Unavailable': 'Недоступно',
            'Search': 'Поиск',
            'User Information': 'Информация Пользователя',
            'No item found': 'Предмет не найден',
            'Create new item': 'Создать новый предмет',
            'Decoded Text': 'Декодированный текст',
            'Format': 'Формат',
            'Scanned results:': 'Отсканированные результаты:',
            'Scanning...': 'Сканирование...',
            'Clear': 'Очистить',
            'Scan': 'Скан',
            'Phone:': 'Номер Телефона:',
            'Search by category': 'Поиск по категориям',
            'This function was created to find appropriate classrooms with certain number of items per category.': 'Эта функция была создана для поиска соответствующих аудиторий с определенным количеством предметов на категорию.',
            'Enter desired number of items:': 'Введите требуемое количество позиций:',
            'Classroom name/number': 'Название/номер класса',
            'Number of': 'Количество',
            'Find': 'Найти',
            'Profile': 'Профиль',
            'Generate report': 'Сгенерируйте отчет',
            '>Add icon<': '>Добавьте иконку<',
            'No images...': 'Нет изображений...',
            'Show PDF':'Показать PDF',
            'Generated reports': 'Сгенерированные отчеты',
            'No reports...': 'Никаких отчетов...',
            'Report time': 'Время отчета',
            'Link': 'Ссылка',
            'Create Item': 'Добавить предмет',
            'Search by item': 'Поиск по предмету',
            'Search by name': 'Поиск по названию',
            'Search by classroom name': 'Поиск по названию кабинета',
            'Add icon': 'Добавить иконку',
            'Add images': 'Добавить изображения',
            'Edit Item': 'Изменить предмет',
            'Deletion is in progress...': 'Выполняется удаление...',
            'Deleted!': 'Удален!',
            'Deletion in progress...': 'Выполняется удаление...',
            'show more images': 'показать больше',
            'show less images': 'показать меньше',
            'Incorrect credentials data!': 'Неверные данные!',
            'Creation is in progress...': 'Идет создание...',
            'Added!': 'Добавлено!',
            'Editing is in progress...': 'Идет редактирование...',
            'Edited!': 'Отредактировано!',
            'Classroom creation is in progress...': 'Идет создание кабинета...',
            'Classroom was added!': 'Добавлен кабинет!',
            'Classroom was edited!': 'Класс был отредактирован!',
            'Searching...': 'Поиск...',
            'No result': 'Нет результата',
            'Found!': 'Найден!',
            'Adding your item...': 'Добавление элемента...',
            'Saving your changes...': 'Сохранение изменений...',
            'Saved!': 'Сохранен!',
            'You can check your report in profile!': 'Вы можете проверить свой отчет в профиле!',
            'Quantity of items per category.': 'Количество элементов по категориям.',
            'No information': 'Никакой информации',
            'User was added!': 'Пользователь добавлен!',
            'Credentials were sent to the email! (Check spam box)': 'Учетные данные были отправлены на адрес электронной почты! (Просмотрите вкладку «спам»)',
            'Download Excel': 'Загрузить Excel',
            'Extracting logs...': 'Извлечение журналов...',
            'Logs were extracted from server!': 'Журналы были извлечены с сервера!',
            'Open logs': 'Открыть журналы',
            'Get logs': 'Получить журналы',
            'Something went wrong...': 'Что-то пошло не так...',
            'Count': 'Количество',
            'The management of inventory in university housing is complicated by the arrangement and monitoring of resources across various rooms and facilities. The purpose of this project is to develop a client-server application that addresses these problems in a user-friendly and efficient manner. The application makes use of cutting-edge tools including ReactJS, ASP.NET, PostgreSQL, and AWS for hosting.': 'Управление инвентарем в университетском корпусе осложняется размещением и мониторингом ресурсов в различных помещениях и приспособлениях. Целью этого проекта является разработка клиент-серверного приложения, которое решает эти проблемы удобным для пользователя и эффективным способом. Приложение использует передовые инструменты, включая ReactJS, ASP.NET , PostgreSQL и AWS для хостинга.',
            'The appealing and straightforward user interface of the front-end component allows users to add, edit, and remove classes and commodities. The back-end component, which also interacts with the database and ensures efficient data retrieval and storage, is in charge of handling the business logic. The front-end and back-end components are connected through HTTP calls, allowing for real-time modifications and a seamless user experience.': 'Привлекательный и простой пользовательский интерфейс интерфейсного компонента позволяет пользователям добавлять, редактировать и удалять классы и товары. Серверный компонент, который также взаимодействует с базой данных и обеспечивает эффективный поиск и хранение данных, отвечает за обработку бизнес-логики. Интерфейсный и серверный компоненты подключаются посредством HTTP-вызовов, что позволяет вносить изменения в режиме реального времени и обеспечивает бесперебойный пользовательский интерфейс.',
            'Empty fields!': 'Пустые поля!',
            'Too much data!': 'Много данных!',
            'Create Item from Scanner': 'Добавить предмет из сканнера',
            'Name should be at least 2 characters!': 'В названии должно быть как минимум 2 символа!'
        }
    }
}

i18next.
    use(initReactI18next).
    init({
    resources: resourses, lng: localStorage.getItem('lang') ,interpolation: {
        escapeValue: false
    }
})

export default i18next;