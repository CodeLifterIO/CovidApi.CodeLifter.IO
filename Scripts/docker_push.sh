echo '--- Please enter the last processed file minus the .csv'
read fileData
echo "Creating migration tagged ${fileData}"

echo 'PUSHING covidapi-codelifter-data'
container_name=covidapi-codelifter-data
docker commit -m "A working SQL Server container storing all the John's Hopkins Covid Data as of ${fileData}" -a "Andrew Palmer (CodeLifterIO)" ${container_name} codelifterio/${container_name}:${fileData} 
docker push codelifterio/${container_name}:latest
docker push codelifterio/${container_name}:${fileData} 

echo 'PUSHING covidapi-codelifter-io'
container_name=covidapi-codelifter-io
docker commit -m "A working RESTful API to query all of Johns Hopkins Covid19 data as of ${fileData}.  Hard Dependency on codelifterio/covidapi-codelifter-data" -a "Andrew Palmer (CodeLifterIO)" ${container_name} codelifterio/${container_name}:${fileData} 
docker push codelifterio/${container_name}:latest 
docker push codelifterio/${container_name}:${fileData} 

echo 'PUSHING covidapi-codelifter-admin'
container_name=covidapi-codelifter-admin
docker commit -m "A working admin tool to download all of Johns Hopkins Covid19 data as of ${fileData}.  Hard Dependency on codelifterio/covidapi-codelifter-data" -a "Andrew Palmer (CodeLifterIO)" ${container_name} codelifterio/${container_name}:${fileData} 
docker push codelifterio/${container_name}:latest
docker push codelifterio/${container_name}:${fileData} 