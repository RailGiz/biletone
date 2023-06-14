<!DOCTYPE html>
<html>
<head>
    <title>Загрузка и просмотр фотографий</title>
</head>
<body>
    <h2>Загрузка фотографии</h2>
    <form action="index.php" method="post" enctype="multipart/form-data" autocomplete="off">
        <input type="file" name="photo">
        <input type="submit" name="submit" value="Загрузить">
    </form>

    <h2>Загруженные фотографии</h2>
    <a href="uploaded.php">Перейти к загруженным фотографиям</a>

    <?php
    $uploadDir = 'uploads/';

    if ($_SERVER['REQUEST_METHOD'] === 'POST') {
        if (isset($_FILES['photo'])) {
            if ($_FILES['photo']['error'] === UPLOAD_ERR_OK) {
                $filename = uniqid('photo_') . '.jpg';
                if (move_uploaded_file($_FILES['photo']['tmp_name'], $uploadDir . $filename)) {
                    echo 'Файл успешно загружен и сохранен на сервере.';
                } else {
                    echo 'Произошла ошибка при сохранении файла.';
                }
            } else {
                echo 'Произошла ошибка при загрузке файла: ' . $_FILES['photo']['error'];
            }
        }
    }
    ?>
</body>
</html>
