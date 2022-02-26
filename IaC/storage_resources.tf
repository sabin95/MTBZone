resource "aws_db_instance" "MTBZoneDB" {
  allocated_storage    = 20
  engine               = "sqlserver-ex"
  engine_version       = "14.00.3401.7.v1"
  instance_class       = "db.t2.micro"
  username             = var.db_username
  password             = var.db_password
  skip_final_snapshot  = true
  license_model = "license-included"
  publicly_accessible = true
  identifier = "mtbzone-db"
  db_subnet_group_name   = aws_db_subnet_group.MTBZoneDBSubnetGroup.name
  vpc_security_group_ids = [aws_security_group.MTBZoneDBSecurityGroup.id]
}

resource "aws_db_subnet_group" "MTBZoneDBSubnetGroup" {
  name       = "mtbzone-db-subnet-group"
  subnet_ids = aws_subnet.MTBZoneDBSubnet[*].id

  tags = {
    Name = "mtbzone-db-subnet-group"
  }
}

resource "aws_security_group" "MTBZoneDBSecurityGroup" {
  name        = "MTBZoneDBSecurityGroup"
  vpc_id      = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneDBSecurityGroup"
  }
}

data "aws_availability_zones" "available" {
  state = "available"
}

resource "aws_subnet" "MTBZoneDBSubnet" {
  count = 3
  vpc_id     = aws_vpc.MTBZoneVPC.id
  cidr_block = "10.0.${count.index}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  tags = {
    Name = "MTBZoneDBSubnet"
  }
}

resource "aws_route_table_association" "MTBZoneRouteTableToDBSubnet" {
  count = length(aws_subnet.MTBZoneDBSubnet)
  route_table_id = aws_route_table.MTBZoneRouteTable.id
  subnet_id      = aws_subnet.MTBZoneDBSubnet[count.index].id
}

resource "aws_security_group_rule" "MTBZoneDBSecurityGroupLambdaIngressRule" {
  type              = "ingress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneLambdaSecurityGroup.id
}

resource "aws_security_group_rule" "MTBZoneLambdaSecurityGroupDBEgressRule" {
  type              = "egress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneLambdaSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
}