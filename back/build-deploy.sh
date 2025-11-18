#!/bin/bash

set -e  # Прерывать выполнение при ошибках

# Определение переменных
PROJECT_NAME="upgrader"
TAR_FILE="$PROJECT_NAME.tar.gz"
TAR_FILE_LOCAL="publish/$PROJECT_NAME.tar.gz"
DEPLOY_DIR="/var/deploy/$PROJECT_NAME"
SERVICE_NAME="$PROJECT_NAME.service"
REMOTE_USER="root"
REMOTE_HOST="178.63.82.62"
REMOTE_PATH="/var/deploy"
SSH_PORT=222

BUILD_DIR="publish"

rm -rf $BUILD_DIR/*
rm -f $TAR_FILE

# Публикация .NET приложения
dotnet publish Upgrader/Upgrader.csproj -c Release -o $BUILD_DIR

rm -rf $BUILD_DIR/appsettings.*

# Архивация собранного билда в TAR.GZ
tar -czf $TAR_FILE -C $BUILD_DIR .

# Передача файлов на удалённый сервер
scp -P $SSH_PORT $TAR_FILE $REMOTE_USER@$REMOTE_HOST:$REMOTE_PATH

# Остановка сервиса, удаление старой версии, разархивирование и запуск новой версии
ssh -p $SSH_PORT $REMOTE_USER@$REMOTE_HOST << EOF
    sudo systemctl stop $SERVICE_NAME
    sudo rm -rf $DEPLOY_DIR
    sudo mkdir -p $DEPLOY_DIR
    sudo tar -xzf $REMOTE_PATH/$TAR_FILE -C $DEPLOY_DIR
	sudo rm -f $DEPLOY_DIR/appsettings.*
    sudo cp $REMOTE_PATH/configurations/${PROJECT_NAME}_appsettings.json $DEPLOY_DIR/appsettings.json
	#sudo npm install --prefix "$DEPLOY_DIR"
    sudo systemctl start $SERVICE_NAME
    sudo systemctl status $SERVICE_NAME --no-pager
EOF

echo "Deploy end."
