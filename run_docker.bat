REM script for quick demo and quick InterSystems IRIS image testing


set container_image="ers/tie"

REM the docker run command
docker run -d -p 9091:1972 -p 9092:52773 -p 9093:52773 -v /data/durable:/dur -h iris --name iris --cap-add IPC_LOCK --env ISC_DATA_DIRECTORY=/dur/iris_conf.d --env ISC_CPF_MERGE_FILE=/dur/merge/merge.cpf %container_image% 