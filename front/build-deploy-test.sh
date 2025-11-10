#!/bin/bash

# Поставьте useLocalEndpoints = false в config.ts перед деплоем

set -e  # Прерывать выполнение при ошибках

# Определение переменных
PROJECT_NAME="hh"
TAR_FILE="$PROJECT_NAME.tar.gz"
TAR_FILE_LOCAL="publish/$PROJECT_NAME.tar.gz"
DEPLOY_DIR="/var/deploy/$PROJECT_NAME"
REMOTE_USER="root"
REMOTE_HOST="178.63.82.62"
REMOTE_PATH="/var/deploy"
SSH_PORT=222

npm run build

rm -f publish/$TAR_FILE

mkdir -p publish

tar -czf publish/$TAR_FILE -C dist .

# Передача файлов на удалённый сервер
scp -P $SSH_PORT $TAR_FILE_LOCAL $REMOTE_USER@$REMOTE_HOST:$REMOTE_PATH

# Остановка сервиса, удаление старой версии, разархивирование и запуск новой версии
ssh -p $SSH_PORT $REMOTE_USER@$REMOTE_HOST << EOF
    sudo rm -rf $DEPLOY_DIR
    sudo mkdir -p $DEPLOY_DIR
    sudo tar -xzf $REMOTE_PATH/$TAR_FILE -C $DEPLOY_DIR
EOF


echo "Deploy end."
