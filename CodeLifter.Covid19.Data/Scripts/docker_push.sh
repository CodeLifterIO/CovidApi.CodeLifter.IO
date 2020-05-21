echo '--- Please enter the last processed file minus the .csv'
read fileData
echo "Creating migration tagged ${fileData}"

container_name=covidapi-codelifter-data

docker commit -m "A working SQL Server container storing all the John's Hopkins Covid Data as of ${fileData}" -a "Andrew Palmer (CodeLifterIO)" ${container_name} codelifterio/${container_name}:${fileData} 
docker push codelifterio/${container_name}:${fileData} 
