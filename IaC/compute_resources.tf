resource "aws_vpc" "MTBZoneVPC" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name = "MTBZoneVPC"
  }
  enable_dns_hostnames = true
  enable_dns_support = true
}

resource "aws_internet_gateway" "MTBZoneiGW" {
  vpc_id = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneiGW"
  }
}

resource "aws_route_table" "MTBZoneRouteTable" {
  vpc_id = aws_vpc.MTBZoneVPC.id  

  tags = {
    Name = "MTBZoneRouteTable"
  }
}

resource "aws_route" "MTBZoneRouteTableIGWRoute" {
  route_table_id          = aws_route_table.MTBZoneRouteTable.id
  destination_cidr_block  = "0.0.0.0/0"
  gateway_id              = aws_internet_gateway.MTBZoneiGW.id
}

resource "aws_subnet" "MTBZoneLambdaSubnet" {
  vpc_id     = aws_vpc.MTBZoneVPC.id
  cidr_block = "10.0.4.0/24"

  tags = {
    Name = "MTBZoneLambdaSubnet"
  }
}

resource "aws_security_group" "MTBZoneLambdaSecurityGroup" {
  name        = "MTBZoneLambdaSecurityGroup"
  vpc_id      = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneLambdaSecurityGroup"
  }
}