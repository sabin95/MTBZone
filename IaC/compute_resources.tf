data "aws_region" "current" {}

resource "aws_vpc" "MTBZoneVPC" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name = "MTBZoneVPC"
  }
  enable_dns_hostnames = true
  enable_dns_support   = true
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
  route_table_id         = aws_route_table.MTBZoneRouteTable.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.MTBZoneiGW.id
}

resource "aws_subnet" "MTBZoneLambdaSubnet" {
  count             = 3
  vpc_id            = aws_vpc.MTBZoneVPC.id
  cidr_block        = "10.0.${count.index + 3}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  tags = {
    Name = "MTBZoneLambdaSubnet"
  }
}

resource "aws_route_table_association" "MTBZoneRouteTableToLambdaSubnet" {
  count          = length(aws_subnet.MTBZoneDBSubnet)
  route_table_id = aws_route_table.MTBZoneRouteTable.id
  subnet_id      = aws_subnet.MTBZoneLambdaSubnet[count.index].id
}

resource "aws_subnet" "MTBZoneSNSEndpointSubnet" {
  count             = 3
  vpc_id            = aws_vpc.MTBZoneVPC.id
  cidr_block        = "10.0.${count.index + 6}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  tags = {
    Name = "MTBZoneSNSEndpointSubnet"
  }
}

resource "aws_security_group" "MTBZoneLambdaSecurityGroup" {
  name   = "MTBZoneLambdaSecurityGroup"
  vpc_id = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneLambdaSecurityGroup"
  }
}

resource "aws_security_group" "MTBZoneSNSEndpointSecurityGroup" {
  name   = "MTBZoneSNSEndpointSecurityGroup"
  vpc_id = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneSNSEndpointSecurityGroup"
  }
}

resource "aws_vpc_endpoint" "SNSEndpoint" {
  vpc_id            = aws_vpc.MTBZoneVPC.id
  service_name      = "com.amazonaws.${data.aws_region.current.name}.sns"
  vpc_endpoint_type = "Interface"

  subnet_ids = aws_subnet.MTBZoneSNSEndpointSubnet[*].id

  security_group_ids = [
    aws_security_group.MTBZoneSNSEndpointSecurityGroup.id,
  ]

  private_dns_enabled = true
}

resource "aws_security_group_rule" "MTBZoneSNSEndpointSecurityGroupIngressLambdaRule" {
  type                     = "ingress"
  from_port                = 443
  to_port                  = 443
  protocol                 = "tcp"
  security_group_id        = aws_security_group.MTBZoneSNSEndpointSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneLambdaSecurityGroup.id
}


resource "aws_security_group_rule" "MTBZoneSNSEndpointSecurityGroupEgressLambdaRule" {
  type                     = "egress"
  from_port                = 443
  to_port                  = 443
  protocol                 = "tcp"
  security_group_id        = aws_security_group.MTBZoneLambdaSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneSNSEndpointSecurityGroup.id
}
