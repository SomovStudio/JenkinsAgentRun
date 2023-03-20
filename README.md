# JenkinsAgentRun
Программа для удобного запуска Jenkins агента

Создание Jenkins агента
===============================================================
1. Открыть "Настройки Jenkins" http://localhost:8080/manage
2. Перейти в "Глобальные настройки безопасности" http://localhost:8080/configureSecurity/
3. В разделе Agents включить флаг TCP port for JNLP agents в состояние Случайный/Random
4. Сохранить настройки
5. Вернуться в "Настройки Jenkins" http://localhost:8080/manage
6. Перейти в "Управление средами сборки" http://localhost:8080/computer/
7. Нажать на кнопку "Новый узел" (New Node)
7.1 ввести наименование (например: proxy)
7.2 включить флаг Permanent Agent
7.3 нажать ОК
7.4 Описание: Тестирование с помощью BrowserMob Proxy
7.5 Количество процессов-исполнителей: 1
7.6 Корень удаленной ФС: C:\Program Files (x86)\Jenkins\workspace_proxy
7.7 Метки: proxy
7.8 Использование: Use this node as much as possible
7.9 Способ запуска: Launch agent via Java Web Start (все параметры оставить по умолчанию)
7.10 Доступность: Keep this agent online as much as possible
7.11 Node Properties: не включать флаги
7.12 нажать кнопку Save
8. Вернуться в "Управление средами сборки" http://localhost:8080/computer/
9. В таблице нажать на proxy


Запуск агента (программа JenkinsAgentRun)
===============================================================
10. Jenkins предлогает несколько способов запуска агента
10.1 способ №1: выполнить команду javaws http://localhost:8080/computer/proxy/slave-agent.jnlp
10.2 способ №2: скачать файл agent.jar по ссылке http://localhost:8080/jnlpJars/agent.jar и выполнить команду
java -jar agent.jar -jnlpUrl http://localhost:8080/computer/proxy/slave-agent.jnlp -secret 0000000000000000000000000000000000000000000000000000000000000000 -workDir "C:\Program Files (x86)\Jenkins\workspace_proxy"

11. Запуск агента с помощью JenkinsAgentRun
11.1 создайне папку, например C:\Jenkins и поместите в папку файлы:
agent.jar
slave-agent.jnlp
JenkinsAgentRun.exe
11.2 запустите программу JenkinsAgentRun.exe она создаст файл config.txt
11.3 введите в поля ранее полученные данные (пункт 10) и нажмите в меню "Файл" пункт "Сохранить настройки"
- путь к приложению (java.exe)
- параметры языка (-Dfile.encoding=UTF8) для поддержки русского языка
- путь к файлу agent.jar (http://localhost:8080/jnlpJars/agent.jar)
- путь к файлу slave-agent.jnlp (http://localhost:8080/computer/proxy/slave-agent.jnlp)
- укажите secret
- укажите рабочую папку workDir
11.4 выполните запуск агента с помощью менб "Действие" пункт "Запустить агента"
Если все поля были правильно заполнены агент будет запущен


Настройка задачи для использования агента
===============================================================
12. Настройка Job для работы с агентом
12.1 включить параметр "Ограничить лейблы сборщиков, которые могут исполнять данную задачу"
12.2 в поле Label Expression ввести метку proxy