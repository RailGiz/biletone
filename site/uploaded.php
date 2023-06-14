<!DOCTYPE html>
<html>
<head>
    <title>Загруженные фотографии</title>
</head>
<body>
    <h2>Загруженные фотографии</h2>
    <?php
    $uploadDir = 'uploads/';

    $fileList = scandir($uploadDir);
    if ($fileList !== false) {
        echo '<ul>';
        foreach ($fileList as $file) {
            if ($file === '.' || $file === '..') {
                continue;
            }

            echo '<li><a href="' . $uploadDir . $file . '">' . $file . '</a></li>';
            echo '<img src="' . $uploadDir . $file . '" alt="Фотография" width="300">';
        }
        echo '</ul>';
    } else {
        echo 'Не удалось открыть папку с загруженными фотографиями.';
    }
    ?>
    <br>
    <a href="index.php">Назад</a>
</body>
</html>
