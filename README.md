# VillageGame

Уютная 2D-игра про деревню: исследование, сбор ресурсов, рыбалка, общение с жителями и смена сезонов.

**Стек:** Unity 6000.4.7f1 (Unity 6) · URP 2D · Input System · Cinemachine 3 · Tilemap

## Как запустить

1. Unity Hub → **Add project from disk** → выбрать папку репозитория
2. Открыть `Assets/Scenes/SampleScene.unity`
3. Нажать **Play**

Первый запуск после клонирования долгий — Unity заново генерирует `Library/`. Это нормально, в репозитории её нет.

## Что уже работает

- **Инвентарь** — рюкзак, хотбар, слоты, перетаскивание (`InventoryManager`, `BackpackUI`, `HotbarController`)
- **Предметы** — ScriptableObject в `Assets/_Project/Resources/Items` (`Apple`, `Axe`, `Blueberry`, `Grass`)
- **Рыбалка** — мини-игра и экран поимки (`FishingMinigame`, `FishCatchUI`)
- **Время и сезоны** — игровые сутки, прогресс дня, календарь-журнал (`GameTime`, `DayProgressManager`, `SeasonUI`, `JournalCalendar`)
- **Диалоги** — реплики NPC (`DialogueManager`, `DialogueTrigger`)
- **Сохранения** — `save.json` в `Application.persistentDataPath` (`SaveSystem`)
- **Управление** — `Assets/InputSystem_Actions.inputactions`

## Структура

```
Assets/
  _Project/Scripts/    игровой код (C#)
  _Project/Sprites/    графика (PNG + анимации)
  _Project/Resources/  предметы (ScriptableObject)
  Scenes/              SampleScene — основная сцена
  Settings/            настройки URP
ProjectSettings/       настройки проекта Unity
Packages/              зависимости
```

## Чего в репозитории нет

`Library/`, `Builts/`, `Temp/`, `Logs/`, `UserSettings/` — генерируются Unity локально и в git не попадают.

## Слияние сцен

Файлы `.unity`, `.prefab`, `.asset` помечены как `merge=unityyamlmerge` — конфликты разрешает Unity SmartMerge вместо обычного текстового мерджа. Драйвер настраивается один раз на машине:

```
git config merge.unityyamlmerge.driver "\"C:/Program Files/Unity/Hub/Editor/6000.4.7f1/Editor/Data/Tools/UnityYAMLMerge.exe\" merge -p %O %B %A %A"
```

## Статус

Ранний прототип. Играбельной петли «день → сбор → продажа» пока нет.