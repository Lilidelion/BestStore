pipeline {
    agent any

    stages {
        stage('Clone') {
            steps {
                echo 'Cloning source code'
                git branch: 'main', url: 'https://github.com/LiLyTampone/ProjectCart.git'
            }
        }

        stage('Restore Packages') {
            steps {
                echo 'Restore packages'
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                echo 'Building project (.NET Core)'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Run Tests') {
            steps {
                echo 'Running tests...'
                bat 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish to Local Folder') {
            steps {
                echo 'Publishing to local folder...'
                bat 'dotnet publish -c Release -o ./publish'
            }
        }

        stage('Copy to IIS Folder') {
            steps {
                echo 'Copying to IIS folder...'
                // Nếu muốn stop IIS thì bỏ comment dòng dưới
                // bat 'iisreset /stop'
                bat 'xcopy "%WORKSPACE%\\publish" /E /Y /I /R "c:\\wwwroot\\myproject"'
            }
        }

        stage('Deploy to IIS') {
            steps {
                echo 'Deploying to IIS...'
                powershell '''
                Import-Module WebAdministration

                # Tạo website nếu chưa có
                if (-not (Test-Path IIS:\\Sites\\MySite)) {
                    New-Website -Name "MySite" -Port 81 -PhysicalPath "C:\\wwwroot\\myproject" -Force
                } else {
                    Write-Host "Website 'MySite' already exists."
                }
                '''
            }
        }
    }
}
