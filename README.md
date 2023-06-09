# JenkinsAgentRun
Программа для удобного запуска Jenkins агента
<br>
Скачать: https://github.com/SomovStudio/JenkinsAgentRun/releases/download/release/JenkinsAgentRun-1.1.zip
<br>
(последнее обновление 13.04.2023 версия 1.1)
<hr>
<p align="center">
  <img src="https://github.com/SomovStudio/JenkinsAgentRun/blob/main/JenkinsAgentRun/JenkinsAgentRun/img/JenkinsAgentRun.png">
</p>

Системные требования: 
<br>
<ul>
	<li>Windows 8, 10, 11</li>
	<li><a href="https://dotnet.microsoft.com/download/dotnet-framework" target="_blank">.NET Framework 4.8</a></li>
</ul>
<br>
<hr>

Создание Jenkins агента
===============================================================
<p align="center">
  <img src="https://github.com/SomovStudio/JenkinsAgentRun/blob/main/JenkinsAgentRun/JenkinsAgentRun/img/builds.jpg">
  <img src="https://github.com/SomovStudio/JenkinsAgentRun/blob/main/JenkinsAgentRun/JenkinsAgentRun/img/nodes.jpg">
</p>

1. Открыть "Настройки Jenkins" http://localhost:8080/manage
2. Перейти в "Глобальные настройки безопасности" http://localhost:8080/configureSecurity/
3. В разделе Agents включить флаг TCP port for JNLP agents в состояние Случайный/Random
4. Сохранить настройки
5. Вернуться в "Настройки Jenkins" http://localhost:8080/manage
6. Перейти в "Управление средами сборки" http://localhost:8080/computer/
7. Нажать на кнопку "Новый узел" (New Node)
- наименование (Name) (например: agent)
- включить флаг Permanent Agent
- нажать ОК

- Количество процессов-исполнителей (of executors): 2
- Корень удаленной папки (Remote root directory): C:\Jenkins\workspace
- Метки (Label): agent
- Использование (Usage): Only build jobs with label expressions matching this node 
- Способ запуска (Launch method): Launch agent by connecting it to the master (все параметры оставить по умолчанию)
- Доступность (Availability): Keep this agent online as much as possible
- Node Properties: не включать флаги
- нажать кнопку Save

8. Вернуться в "Управление средами сборки" http://localhost:8080/computer/
9. В таблице нажать на agent чтобы просмотреть настройки агента


Запуск агента (программа JenkinsAgentRun)
===============================================================
<p align="center">
  <img src="https://github.com/SomovStudio/JenkinsAgentRun/blob/main/JenkinsAgentRun/JenkinsAgentRun/img/run.jpg">
</p>

10. Jenkins предлогает свой способ запуска агента с помощью команды

java -jar agent.jar -jnlpUrl http://localhost:8080/computer/agent/slave-agent.jnlp -secret 00000000000000000000000000000000 -workDir "C:\Jenkins\workspace"


11. Запуск агента с помощью JenkinsAgentRun
- создайне папку, например C:\Jenkins и поместите в папку файлы: agent.jar, slave-agent.jnlp, JenkinsAgentRun.exe
- запустите программу JenkinsAgentRun.exe (от имени администратора) она создаст файл config.txt
- введите в поля ранее полученные данные (пункт 10) и нажмите в меню "Файл" пункт "Сохранить настройки"
- выполните запуск агента с помощью меню "Действие" пункт "Запустить агента"

Если все поля были правильно заполнены агент будет запущен


Настройка задачи для использования агента
===============================================================
<p align="center">
  <img src="https://github.com/SomovStudio/JenkinsAgentRun/blob/main/JenkinsAgentRun/JenkinsAgentRun/img/label.jpg">
</p>

12. Настройка Job для работы с агентом
- включить параметр "Ограничить лейблы сборщиков, которые могут исполнять данную задачу"
- в поле Label Expression ввести метку agent